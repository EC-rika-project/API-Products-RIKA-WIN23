using Microsoft.EntityFrameworkCore;
using Products.Api.DAL;
using Products.Api.Entities;
using Products.Api.Models.Requests;
using Products.Api.Services;

namespace Products.Api.Tests.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetCategoriesAsync_ReturnsCategories()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new DataContext(options);
        context.Categories.AddRange(new List<CategoryEntity>
        {
            new() { Name = "Category1", Description = "Desc1"},
            new() { Name = "Category2", Description = "Desc2"}
        });
        await context.SaveChangesAsync();

        var service = new CategoryService(context);

        // Act
        var result = await service.GetCategoriesAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        Assert.Contains(result.Data, c => c.Name == "Category1");
        Assert.Contains(result.Data, c => c.Name == "Category2");
        Assert.Contains(result.Data, c => c.Description == "Desc1");
        Assert.Contains(result.Data, c => c.Description == "Desc2");
    }

    [Fact]
    public async Task GetCategoriesAsync_ReturnsEmptyList_WhenNoCategories()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        await using var context = new DataContext(options);
        var service = new CategoryService(context);

        // Act
        var result = await service.GetCategoriesAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
    }


    [Fact]
    public async Task CreateCategoryAsync_ReturnsSuccess_WhenCategoryIsCreated()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        var service = new CategoryService(context);

        var categoryRequest = new CategoryRequest
        {
            Name = "NewCategory",
            Description = "New Description"
        };

        // Act
        var result = await service.CreateCategoryAsync(categoryRequest);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("NewCategory", result.Data.Name);
        Assert.Equal("New Description", result.Data.Description);
    }


    [Fact]
    public async Task CreateCategoryAsync_ReturnsFailure_WhenCategoryNameAlreadyExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);

        
        context.Categories.Add(new CategoryEntity
        {
            Name = "DuplicateCategory",
            Description = "Existing Description"
        });
        await context.SaveChangesAsync();

        var service = new CategoryService(context);
        var categoryRequest = new CategoryRequest
        {
            Name = "DuplicateCategory",
            Description = "New Description"
        };

        // Act
        var result = await service.CreateCategoryAsync(categoryRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Category already exists.", result.ErrorMessage);
    }

    [Fact]
    public async Task CreateCategoryAsync_ReturnsFailure_WhenCategoryNameNoMatterCaseAlreadyExists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new DataContext(options);
        
        context.Categories.Add(new CategoryEntity
        {
            Name = "DuplicateCategory",
            Description = "Existing Description"

        });
        await context.SaveChangesAsync();

        var service = new CategoryService(context);
        var categoryRequest = new CategoryRequest
        {
            Name = "duplicateCategory",
            Description = "New Description"
        };

        // Act
        var result = await service.CreateCategoryAsync(categoryRequest);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Category already exists.", result.ErrorMessage);
    }
}