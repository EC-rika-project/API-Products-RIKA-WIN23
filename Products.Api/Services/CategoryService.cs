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
        if (await dataContext.Categories.AnyAsync(x => x.Name.ToLower() == categoryRequest.Name.ToLower()))
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
            return ServiceResult<Category>.Failure("An unexpected error occured");
        }

        var createdCategory = new Category
        {
            Name = categoryRequest.Name,
            Description = categoryRequest.Description
        };

        return ServiceResult<Category>.Success(createdCategory);
    }

    public async Task<ServiceResult<object>> DeleteCategoryAsync(string categoryName)
    {
        if (await dataContext.Products.AnyAsync(x => x.CategoryName.ToLower() == categoryName.ToLower()))
        {
            return ServiceResult<object>.Failure("Category contains products!");
        }
        var category = await dataContext.Categories.FirstOrDefaultAsync(x => x.Name.Equals(categoryName, StringComparison.CurrentCultureIgnoreCase));

        if (category == null)
        {
            return ServiceResult<object>.Failure("Category does not exist!");
        }
        
        dataContext.Categories.Remove(category);
        await dataContext.SaveChangesAsync();
        return ServiceResult<object>.Success("Category deleted!");
    }
}