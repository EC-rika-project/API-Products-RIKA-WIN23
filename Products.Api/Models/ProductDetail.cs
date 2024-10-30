using Products.Api.Entities;

namespace Products.Api.Models;


public class ProductResponse
{
    public ProductDetail? Product { get; set; }
    public List<ProductDetail> Variants { get; set; } = [];
    
    public static ProductResponse ToProductResponse(ProductEntity product, List<ProductEntity> variants)
    {
        return new ProductResponse
        {
            Product = ProductDetail.ToProductDetail(product),
            Variants = variants.Select(ProductDetail.ToProductDetail).ToList() 
        };
    }
}

public class ProductDetail
{
    public List<string> ImageUrls { get; set; } = [];
    public string Description { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Color { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string ArticleNumber { get; set; } = null!;
    public List<ProductVariation> Variations { get; set; } = [];
    public decimal Price { get; set; }
    public string CoverImageUrl { get; set; } = null!;

    public static ProductDetail ToProductDetail(ProductEntity product)
    {
        return new ProductDetail
        {
            //ImageUrls = product.ImageUrls, // This is saved for later.
            Description = product.Description,
            Name = product.Name,
            Color = product.Color,
            Ingress = product.Ingress,
            CategoryName = product.CategoryName,
            ArticleNumber = product.ArticleNumber,
            Variations = product.Variations.Select(x => new ProductVariation
            {
                Name = x.Name,
                Stock = x.Stock
            }).ToList(),
            Price = product.Price,
            CoverImageUrl = product.CoverImageUrl
        };
    }
}

public class ProductVariation
{
    public string Name { get; set; }
    public int Stock { get; set; }
}