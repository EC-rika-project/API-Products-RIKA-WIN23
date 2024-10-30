using Microsoft.AspNetCore.Mvc;
using Products.Api.Services;

namespace Products.Api.Endpoints;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder builder)
    {

        builder.MapGet("/products", async ([FromQuery] string category, [FromQuery] string filter, [FromQuery] int page, [FromQuery] int size, CategoryService categoryService) =>
        {
            var result = await productService.GetProducts(category, filter, page, size);
            return Results.Ok(result.Data);
        });

        return builder;
    }
}
