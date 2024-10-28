namespace Products.Api.Entities;

public class ProductGroupEntity
{
    public Guid Id { get; set; }
    public string CategoryName { get; set; } = null!;
    public CategoryEntity Category { get; set; } = null!;
    public ICollection<ProductEntity> Products { get; set; } = [];
}