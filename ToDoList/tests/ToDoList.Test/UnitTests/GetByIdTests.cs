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
    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false,
            Category = "something"
        };
        // Mock the ReadById method to return the ToDoItem for the valid ID
        repositoryMock.ReadByIdAsync(toDoItem.ToDoItemId).Returns(toDoItem);

        // Act
        var result = await controller.ReadByIdAsync(toDoItem.ToDoItemId);
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
        Assert.Equal(toDoItem.Category, value.Category);
        // Verify that ReadById was called with the correct id
        repositoryMock.Received(1).ReadByIdAsync(toDoItem.ToDoItemId);

    }
    [Fact]
    public async Task Get_ReadByIdWhenSomeItemAvailable_CategoryNull_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false,
            Category = null
        };
        // Mock the ReadById method to return the ToDoItem for the valid ID
        repositoryMock.ReadByIdAsync(toDoItem.ToDoItemId).Returns(toDoItem);

        // Act
        var result = await controller.ReadByIdAsync(toDoItem.ToDoItemId);
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
        Assert.Equal(toDoItem.Category, value.Category);
        // Verify that ReadById was called with the correct id
        repositoryMock.Received(1).ReadByIdAsync(toDoItem.ToDoItemId);

    }

    [Fact]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound()

    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock the ReadById method to return null for an invalid ID
        repositoryMock.ReadByIdAsync(-1).Returns((ToDoItem)null); // Simulate that item with ID -1 does not exist

        // Act
        var invalidId = -1;
        var result = await controller.ReadByIdAsync(invalidId);
        var resultResult = result.Result;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult); // Expecting 404 Not Found
        repositoryMock.Received(1).ReadByIdAsync(invalidId);

    }
   [Fact]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Simulating an exception in the repository's ReadById method
        repositoryMock.When(r => r.ReadByIdAsync(Arg.Any<int>())).Do(x => throw new Exception("Database error"));

        // Act
        var result = await controller.ReadByIdAsync(1); // Using a valid ID here
        var resultResult = result.Result;

        // Assert
        Assert.IsType<ObjectResult>(resultResult); // Expecting 500 Internal Server Error
        var objectResult = resultResult as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);

        // Verify that ReadById was called exactly once
        repositoryMock.Received(1).ReadByIdAsync(1);
    }
}
