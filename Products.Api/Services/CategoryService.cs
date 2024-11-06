using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Models;
using Products.Api.Models.Requests;

namespace Products.Api.Services;

public class CategoryService(DataContext dataContext)
{
    public async Task<ServiceResult<List<Category>>> GetCategoriesAsync()
    {
        var categories = await dataContext.Categories.ToListAsync();
        return ServiceResult<List<Category>>.Success(categories.Select(x => new Category
        {
            Name = x.Name,
            Description = x.Description
        }).ToList());
    }

    public async Task<ServiceResult<Category>> CreateCategoryAsync(CategoryRequest categoryRequest)
    {
        if (await dataContext.Categories.AnyAsync(x => x.Name.Equals(categoryRequest.Name, StringComparison.CurrentCultureIgnoreCase)))
        {
            return ServiceResult<Category>.Failure("Category already exists.");
        }
        
        var categoryEntity = new CategoryEntity
        {
            Name = categoryRequest.Name,
            Description = categoryRequest.Description
        };


        try
        {
            dataContext.Categories.Add(categoryEntity);
            await dataContext.SaveChangesAsync();
        }
        catch (Exception)
        {

            // Hantera andra databasfel med ett generellt felmeddelande
            return ServiceResult<Category>.Failure("An unexpected error occured");
        }

        var createdCategory = new Category
        {
            Name = categoryRequest.Name,
            Description = categoryRequest.Description
        };

        return ServiceResult<Category>.Success(createdCategory);
    }
}