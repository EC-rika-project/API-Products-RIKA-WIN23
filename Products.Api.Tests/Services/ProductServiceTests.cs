using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Models.Requests;
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
    
    [Fact]
    public async Task CreateProductAsync_ReturnsFailure_WhenProductAlreadyExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);

        context.Products.Add(new ProductEntity
        {
            ArticleNumber = "1234",
            ProductGroupId = default,
            ProductGroup = null,
            Name = "Existing Product",
            Description = "Product Description",
            Price = 10,
            Color = "Red",
            CoverImageUrl = "https://hans-tommy.com/image.jpg",
            Ingress = "Short description",
            CategoryName = "Shoes"


        });
        await context.SaveChangesAsync();

        var service = new ProductService(context);
        var productRequest = new ProductRequest { ArticleNumber = "1234", Name = "New Product" };

        // Act
        var result = await service.CreateProductAsync(productRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Product already exists", result.ErrorMessage);
    }
    
    [Fact]
    public async Task CreateProductAsync_ReturnsSuccess_WhenProductIsCreated()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        var service = new ProductService(context);

        var productRequest = new ProductRequest
        {
            ArticleNumber = "1234",
            Name = "New Product",
            Description = "Product Description",
            ProductGroupId = Guid.NewGuid(),
            Color = "Red",
            CoverImageUrl = "https://hans-tommy.com/image.jpg",
            Ingress = "Short description",
            Price = 34+35,
            CategoryName = "Shoes",
            ProductVariations = new List<ProductVariationRequest>
            {
                new() { Name = "Size M", Stock = 10 }
            }
        };

        // Act
        var result = await service.CreateProductAsync(productRequest);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("1234", result.Data.ArticleNumber);
        Assert.Equal("New Product", result.Data.Name);
        Assert.Equal("Short description", result.Data.Ingress);
        Assert.Equal(69, result.Data.Price);
        Assert.Equal("https://hans-tommy.com/image.jpg", result.Data.CoverImageUrl);
    }
    
    [Fact]
    public async Task CreateProductAsync_ReturnsSuccess_WhenProductGroupNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        var service = new ProductService(context);

        var productRequest = new ProductRequest
        {
            ArticleNumber = "1234",
            Name = "Product Without Group",
            CoverImageUrl = "https://hans-tommy.com/image.jpg",
            Description = "Description",
            Price = 0,
            Color = "Red",
            Ingress = "Short Description",
            CategoryName = "Shoes",
            ProductGroupId = Guid.NewGuid()
        };

        // Act
        var result = await service.CreateProductAsync(productRequest);

        // Assert
        Assert.True(result.IsSuccess);
        var createdGroup = await context.ProductGroups.ToListAsync();
        Assert.NotEmpty(createdGroup);
    }
    
    [Fact]
    public async Task CreateProductAsync_ReturnsSuccess_WhenProductGroupId_NotIncluded()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        var service = new ProductService(context);

        var productRequest = new ProductRequest
        {
            CoverImageUrl = "https://hans-tommy.com/image.jpg",
            ArticleNumber = "1234",
            Name = "Product Without Group",
            Description = "Description",
            Price = 0,
            Color = "Red",
            Ingress = "Short Description",
            CategoryName = "Shoes"
        };

        // Act
        var result = await service.CreateProductAsync(productRequest);

        // Assert
        Assert.True(result.IsSuccess);
        var createdGroup = await context.ProductGroups.ToListAsync();
        Assert.NotEmpty(createdGroup);
    }
        
    [Fact]
    public async Task DeleteProductAsync_ReturnsSuccess_WhenProductExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);

        var product = new ProductEntity
        {
            ArticleNumber = "1234",
            Name = "test product",
            Description = "Description",
            Price = 100,
            Color = "Red",
            CoverImageUrl = "https://hans-tommy.jpg",
            Ingress = "Short description",
            CategoryName = "shoes"
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        var service = new ProductService(context);

        // Act
        var result = await service.DeleteProductAsync("1234");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Product with article number: 1234 has been deleted!", result.Data);
        
        var deletedProduct = await context.Products.FirstOrDefaultAsync(p => p.ArticleNumber == "1234");
        Assert.Null(deletedProduct);
    }
    
    [Fact]
    public async Task DeleteProductAsync_ReturnsFailure_WhenProductDoesNotExist()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        var service = new ProductService(context);

        // Act
        var result = await service.DeleteProductAsync("NonExistentArticleNumber");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Product does not exist!", result.ErrorMessage);
    }
} 
