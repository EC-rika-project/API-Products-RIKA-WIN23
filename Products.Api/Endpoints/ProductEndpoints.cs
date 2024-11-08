using Microsoft.AspNetCore.Mvc;
using Products.Api.Models.Requests;
using Products.Api.Services;

namespace Products.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/products", async ([FromQuery] string category, [FromQuery] string? sortBy, [FromQuery] bool? descending, [FromQuery] int page, [FromQuery] int size, ProductService productService) =>
        {
            var result = await productService.GetProductsAsync(category, page, size, sortBy, descending ?? false);
            return Results.Ok(result.Data);
        });
        
        builder.MapGet("/products/{articleNumber}", async ([FromRoute] string articleNumber, ProductService productService) =>
        {
            var result = await productService.GetProductDetailAsync(articleNumber);
            return result.IsSuccess ? Results.Ok(result.Data) : Results.NotFound(result.ErrorMessage);
        });
        
        builder.MapPost("/products", async (ProductRequest productRequest, ProductService productService) =>
        {
            
            var result = await productService.CreateProductAsync(productRequest);
            if (result.IsSuccess)
            {
                return Results.Created($"/products/{result.Data.Name}", result.Data);
            }
            return Results.BadRequest(result.ErrorMessage);
        });

        builder.MapDelete("/products/{articleNumber}", async (string articleNumber, ProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(articleNumber);
            if (result.IsSuccess)
            {
                return Results.Ok(new { message = result.Data });
            }
            return Results.BadRequest(result.ErrorMessage);
        });
        
        return builder;
    }
}
