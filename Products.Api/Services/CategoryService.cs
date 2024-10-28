using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Models;

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
}