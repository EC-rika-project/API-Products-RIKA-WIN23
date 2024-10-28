namespace Products.Api.Entities;

public class CategoryEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ICollection<ProductGroupEntity> ProductGroups { get; set; } = [];
}