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
    
     [Fact]
        public async Task GetProductDetailAsync_ReturnsProductDetail()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new DataContext(options);
            var productGroup = new ProductGroupEntity
            {
                Products = new List<ProductEntity>
                {
                    new ProductEntity
                    {
                        ArticleNumber = "123",
                        Name = "Product1",
                        Description = "Description1",
                        Price = 100,
                        Color = "Red",
                        CoverImageUrl = "url1",
                        Ingress = "Ingress1",
                        CategoryName = "Category1",
                        Variations = new List<ProductVariationEntity>
                        {
                            new ProductVariationEntity { Name = "Variation1", Stock = 10 }
                        }
                    },
                    new ProductEntity
                    {
                        ArticleNumber = "456",
                        Name = "Product1",
                        Description = "Description1",
                        Price = 100,
                        Color = "Red",
                        CoverImageUrl = "url1",
                        Ingress = "Ingress1",
                        CategoryName = "Category1",
                        Variations = new List<ProductVariationEntity>
                        {
                            new ProductVariationEntity { Name = "Variation1", Stock = 10 }
                        }
                    }
                }
            };
            context.ProductGroups.Add(productGroup);
            context.SaveChanges();

            var service = new ProductService(context);

            // Act
            var result = await service.GetProductDetailAsync("123");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data.Product);
            Assert.Equal("123", result.Data.Product.ArticleNumber);
            Assert.Equal("456", result.Data.Variants[0].ArticleNumber);
        }

        [Fact]
        public async Task GetProductDetailAsync_ReturnsFailure_WhenProductNotFound()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var context = new DataContext(options);
            var service = new ProductService(context);

            // Act
            var result = await service.GetProductDetailAsync("NonExistentArticleNumber");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Product not found", result.ErrorMessage);
        }
} 
