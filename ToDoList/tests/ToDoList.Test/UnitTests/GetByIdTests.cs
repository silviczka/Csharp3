namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

public class GetByIdUnitTests
{
    [Fact]
    public void GetById_ValidId_ReturnsItem()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        // Mock the ReadById method to return the ToDoItem for the valid ID
        repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(toDoItem);

        // Act
        var result = controller.ReadById(toDoItem.ToDoItemId);
        var resultResult = result.Result;

        // Assert
        Assert.IsType<OkObjectResult>(resultResult); // Expecting 200 OK
        var okResult = resultResult as OkObjectResult;
        var value = okResult.Value as ToDoItemGetResponseDto;

        Assert.NotNull(value);
        Assert.Equal(toDoItem.ToDoItemId, value.Id);
        Assert.Equal(toDoItem.Description, value.Description);
        Assert.Equal(toDoItem.IsCompleted, value.IsCompleted);
        Assert.Equal(toDoItem.Name, value.Name);

    }

    [Fact]
    public void GetById_InvalidId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock the ReadById method to return null for an invalid ID
        repositoryMock.ReadById(-1).Returns((ToDoItem)null); // Simulate that item with ID -1 does not exist

        // Act
        var invalidId = -1;
        var result = controller.ReadById(invalidId);
        var resultResult = result.Result;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult); // Expecting 404 Not Found
    }
}
