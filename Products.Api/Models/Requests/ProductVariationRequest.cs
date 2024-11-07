namespace Products.Api.Models.Requests;

public class ProductVariationRequest
{
    public string Name { get; set; } = null!;
    public int Stock { get; set; }
}