using Microsoft.EntityFrameworkCore;
using Products.Api.Entities;

namespace Products.Api.DAL;

public class DataContext(DbContextOptions<DataContext> opts) : DbContext(opts)
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<ProductGroupEntity> ProductGroups { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ProductVariationEntity> ProductVariations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryEntity>().HasKey(x => x.Name);
        
        modelBuilder.Entity<ProductGroupEntity>()
            .HasOne<CategoryEntity>(x => x.Category)
            .WithMany(x => x.ProductGroups)
            .HasForeignKey(x => x.CategoryName);
        
     
        
        modelBuilder.Entity<ProductEntity>().HasKey(x => x.ArticleNumber);
        
        modelBuilder.Entity<ProductVariationEntity>()
            .HasOne<ProductEntity>(x => x.Product)
            .WithMany(x => x.Variations)
            .HasForeignKey(x => x.ProductArticleNumber);
    }
}