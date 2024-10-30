namespace Products.Api.Models;

public class Product
{
    public string CoverImageUrl { get; set; } = null!;
    public string ArticleNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Ingress { get; set; } = null!;
    public decimal Price { get; set; }
}
