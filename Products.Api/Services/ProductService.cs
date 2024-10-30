using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Models;

namespace Products.Api.Services;

public class ProductService(DataContext dataContext)
{
    public async Task<ServiceResult<PaginationResult<List<Product>>>> GetProductsAsync(string category, int page = 1, int size = 10,
        string? sortBy = "name", bool descending = false)
    {
        var query = dataContext.Products
            .Where(c => c.CategoryName == category);

        query = sortBy switch
        {
            "price" => descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
            "color" => descending ? query.OrderByDescending(p => p.Color) : query.OrderBy(p => p.Color),
            _ => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name)
        };

        var count = await query.CountAsync();

        var products = await query.Skip((page - 1) * size).Take(size).ToListAsync();

        return ServiceResult<PaginationResult<List<Product>>>.Success(
           new PaginationResult<List<Product>>
           {
               Data =  [
                   ..products.Select(x => new Product
                   {
                       CoverImageUrl = x.CoverImageUrl,
                       ArticleNumber = x.ArticleNumber,
                       Name = x.Name,
                       Ingress = x.Ingress,
                       Price = x.Price
                   })
               ],
               Page = page,
               Size = size,
               Count = count 
           }
        );
    }
}