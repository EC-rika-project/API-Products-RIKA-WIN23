namespace Products.Api.Models.Requests;

public class ProductRequest
{
    public string CoverImageUrl { get; set; } = null!;
    public string ArticleNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Color { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public Guid? ProductGroupId { get; set; }
    public List<ProductVariationRequest> ProductVariations { get; set; } = [];
    public List<string> ImageUrls { get; set; } = [];
}