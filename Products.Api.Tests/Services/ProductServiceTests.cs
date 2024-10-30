using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Services;

namespace Products.Api.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetProductsAsync_ReturnsPaginatedProducts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new DataContext(options);
        context.Products.AddRange(new List<ProductEntity>
        {
            new() { CategoryName = "Electronics", Name = "Product1", ArticleNumber = "art1", Price = 100, CoverImageUrl = "hejhej.se", Description = "Desc", Ingress = "A simple Ingress", Color = "Red" },
            new() { CategoryName = "Electronics", Name = "Product2", ArticleNumber = "art2", Price = 200, CoverImageUrl = "hejhej.se", Description = "Desc", Ingress = "A simple Ingress", Color = "Blue" }
        });
        context.SaveChanges();

        var service = new ProductService(context);

        // Act
        var result = await service.GetProductsAsync("Electronics", page: 1, size: 10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Data.Count);
        Assert.Contains(result.Data.Data, p => p.Name == "Product1");
        Assert.Contains(result.Data.Data, p => p.Name == "Product2");
    }

    [Fact]
    public async Task GetProductsAsync_ReturnsEmptyList_WhenNoProducts()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new DataContext(options);
        var service = new ProductService(context);

        // Act
        var result = await service.GetProductsAsync("NonExistentCategory", page: 1, size: 10);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data.Data);
    }
} 
