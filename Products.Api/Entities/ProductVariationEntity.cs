namespace Products.Api.Entities;

public class ProductVariationEntity
{
    public Guid Id { get; set; }
    public string ProductArticleNumber { get; set; } = null!;
    public ProductEntity Product { get; set; } = null!;
    public string Name { get; set; } = null!; // this is equal to XL or L or M, 128GB etc.
    public int Stock { get; set; }
}