using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Api.Controllers;
using Catalog.Api.Dtos;
using Catalog.Api.Models;
using Catalog.Api.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests;

public class ItemsControllerTests
{
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {        
        // Arrange        
        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Item);
        
        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert        
        //Assert.IsType<NotFoundResult>(result.Result);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        // Arrange
        Item expectedItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(expectedItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        // Assert
        var obj = result.GetObjectResult();
        obj.Should().BeEquivalentTo(
            expectedItem,
            options => options.ComparingByMembers<Item>());

        //result.Value.Should().BeEquivalentTo(expectedItem);
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
        // Arrange
        var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), CreateRandomItem() };

        repositoryStub.Setup(repo => repo.GetItemsAsync())
            .ReturnsAsync(expectedItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.GetItemsAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
        // Arrange
        var allItems = new[] 
        { 
            new Item(){ Name = "Potion"},
            new Item(){ Name = "Antidote"},
            new Item(){ Name = "Hi-Potion"}, 
        };

        var nameToMatch = "Potion";

        repositoryStub.Setup(repo => repo.GetItemsAsync())
            .ReturnsAsync(allItems);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        IEnumerable<ItemDto> foundItems = await controller.GetItemsAsync(nameToMatch);

        // Assert
        foundItems.Should().OnlyContain(
            item => item.Name == allItems[0].Name || item.Name == allItems[2].Name
        );
    }


    [Fact]
    public async Task CreateItemAsync_WithItemToCreate_ReturnsCreatedItem()
    {
        // Arrange
        var itemToCreate = new CreateItemDto(
            Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), 
            rand.Next(1000));

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act     
        var result = await controller.CreateItemAsync(itemToCreate);

        // Assert
        var createdItem = result.GetObjectResult();
        itemToCreate.Should().BeEquivalentTo(
            createdItem,
            options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
        createdItem.Id.Should().NotBeEmpty();
        createdItem.CreatedDate.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);
        
        var itemId = existingItem.Id;
        var itemToUpdate = new UpdateItemDto(
            Guid.NewGuid().ToString(), 
            Guid.NewGuid().ToString(), 
            rand.Next(1000)
        );

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act     
        var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }
    
    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
        // Arrange
        Item existingItem = CreateRandomItem();

        repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
            .ReturnsAsync(existingItem);

        var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

        // Act
        var result = await controller.DeleteItemAsync(existingItem.Id);

        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    private Item CreateRandomItem() 
    {
        return new()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            Price = rand.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }
}