using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Models;

namespace Products.Api.Services;

public class ProductService(DataContext dataContext)
{
    public async Task<ServiceResult<List<Product>>> GetProductsAsync(string category, int page = 1, int size = 10, string sortBy = "price", string sortOrder = "asc")
    {
        

        var query = dataContext.Categories
               .Where(c => c.Name == category)
               .SelectMany(c => c.ProductGroups)
               .SelectMany(pg => pg.Products)
               .Include(p => p.ProductGroup)
               .Include(p => p.Variations);
    }
}
