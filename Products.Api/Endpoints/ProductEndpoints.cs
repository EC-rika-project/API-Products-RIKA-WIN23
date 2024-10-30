using Microsoft.AspNetCore.Mvc;
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

        return builder;
    }
}
