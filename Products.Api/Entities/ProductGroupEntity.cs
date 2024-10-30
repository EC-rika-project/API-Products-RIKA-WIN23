namespace Products.Api.Entities;

public class ProductGroupEntity
{
    public Guid Id { get; set; }
    public ICollection<ProductEntity> Products { get; set; } = [];
}