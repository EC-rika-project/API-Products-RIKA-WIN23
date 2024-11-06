using Products.Api.Models.Requests;
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

        builder.MapPost("/categories", async (CategoryRequest categoryRequest, CategoryService categoryService) =>
        {
            
            var result = await categoryService.CreateCategoryAsync(categoryRequest);
            if (result.IsSuccess)
            {
                return Results.Created($"/categories/{result.Data.Name}", result.Data);
            }
            return Results.BadRequest(result.ErrorMessage);
        });

        builder.MapDelete("/categories/{categoryName}", async (string categoryName, CategoryService categoryService) =>
        {
            var result = await categoryService.DeleteCategoryAsync(categoryName);
            if (result.IsSuccess)
            {
                return Results.Ok();
            }
            return Results.BadRequest(result.ErrorMessage);
        });
        
        return builder;
    }
}