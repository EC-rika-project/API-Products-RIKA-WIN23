namespace Products.Api.Entities;

public class ProductEntity
{
    public string ArticleNumber { get; set; } = null!;
    public Guid ProductGroupId { get; set; }
    public ProductGroupEntity ProductGroup { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public string CoverImageUrl { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public ICollection<ProductVariationEntity> Variations { get; set; } = [];
    public string CategoryName { get; set; } = null!;
    public CategoryEntity Category { get; set; } = null!;
}