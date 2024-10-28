using Products.Api.Services;

namespace Products.Api.Endpoints;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder builder)
    {
        
        builder.MapGet("/categories", async (CategoryService categoryService) =>
        {
            var result = await categoryService.GetCategoriesAsync();
            return Results.Ok(result.Data);
        });

        return builder;
    }
}