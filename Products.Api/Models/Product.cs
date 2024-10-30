namespace Products.Api.Models;

public class Product
{
    public string ImageUrl { get; set; } = null!;
    public string ArticleNumber { get; set; } = null!;
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
}
