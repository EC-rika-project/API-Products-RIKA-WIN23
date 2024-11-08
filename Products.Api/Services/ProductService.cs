using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Models;
using Products.Api.Models.Requests;

namespace Products.Api.Services;

public class ProductService(DataContext dataContext)
{
    public async Task<ServiceResult<PaginationResult<List<Product>>>> GetProductsAsync(string category, int page = 1,
        int size = 10,
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
                Data =
                [
                    ..products.Select(x => new Product
                    {
                        CoverImageUrl = x.CoverImageUrl,
                        ArticleNumber = x.ArticleNumber,
                        Name = x.Name,
                        Ingress = x.Ingress,
                        Price = x.Price,
                        ProductGroupId = x.ProductGroupId
                    })
                ],
                Page = page,
                Size = size,
                Count = count
            }
        );
    }

    public async Task<ServiceResult<ProductResponse>> GetProductDetailAsync(string articleNumber)
    {
        var productGroup = await dataContext.ProductGroups
            .Include(x => x.Products)
            .ThenInclude(x => x.Variations)
            .Where(x => x.Products.Any(p => p.ArticleNumber == articleNumber))
            .ToListAsync();

        var product = productGroup.SelectMany(x => x.Products)
            .FirstOrDefault(x => x.ArticleNumber == articleNumber);
        if (product == null)
        {
            return ServiceResult<ProductResponse>.Failure("Product not found");
        }

        return ServiceResult<ProductResponse>.Success(ProductResponse.ToProductResponse(product,
            productGroup.SelectMany(x => x.Products)
                .Where(x => x.ArticleNumber != product.ArticleNumber)
                .ToList()));
    }

    public async Task<ServiceResult<Product>> CreateProductAsync(ProductRequest productRequest)
    {
        if (await dataContext.Products.AnyAsync(x => x.ArticleNumber.ToLower() == productRequest.ArticleNumber.ToLower()))
        {
            return ServiceResult<Product>.Failure("Product already exists");
        }

        var productGroup =
            await dataContext.ProductGroups.FirstOrDefaultAsync(x => x.Id == productRequest.ProductGroupId);

        if (productGroup == null)
        {
            productGroup = new ProductGroupEntity(){Id = Guid.NewGuid()};
            dataContext.ProductGroups.Add(productGroup);
            await dataContext.SaveChangesAsync();
        }
        
        var productEntity = new ProductEntity
        {
            ArticleNumber = productRequest.ArticleNumber,
            ProductGroupId = productGroup.Id,
            Name = productRequest.Name,
            Description = productRequest.Description,
            Price = productRequest.Price,
            Color = productRequest.Color,
            CoverImageUrl = productRequest.CoverImageUrl,
            Ingress = productRequest.Ingress,
            Variations = productRequest.ProductVariations.Select(x => new ProductVariationEntity
            {
                Id = Guid.NewGuid(),
                ProductArticleNumber = productRequest.ArticleNumber,
                Name = x.Name,
                Stock = x.Stock,
            }).ToList(),
            CategoryName = productRequest.CategoryName,
        };

        
        try
        {
            dataContext.Products.Add(productEntity);
            await dataContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return ServiceResult<Product>.Failure("Could not save product");
        }
        
        return ServiceResult<Product>.Success(new Product
        {
            CoverImageUrl = productEntity.CoverImageUrl,
            ArticleNumber = productEntity.ArticleNumber,
            Name = productEntity.Name,
            Ingress = productEntity.Ingress,
            Price = productEntity.Price,
            ProductGroupId = productEntity.ProductGroupId
        });
    }
    
    public async Task<ServiceResult<object>> DeleteProductAsync(string articleNumber)
    {
        
        var product = await dataContext.Products
            .Include(p =>p.Variations)
            .FirstOrDefaultAsync(x => x.ArticleNumber == articleNumber);

        if (product == null)
        {
            return ServiceResult<object>.Failure("Product does not exist!");
        }
        
        dataContext.Products.Remove(product);
        await dataContext.SaveChangesAsync();
        return ServiceResult<object>.Success($"Product with article number: {articleNumber} has been deleted!");
    }
}