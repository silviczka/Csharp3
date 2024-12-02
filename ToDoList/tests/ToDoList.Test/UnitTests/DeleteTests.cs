using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test;

public class DeleteUnitTests
{
    [Fact]
    public async Task Delete_ValidItemId_ReturnsNoContent_204()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;
        // Mock that item exists
        repositoryMock.ReadByIdAsync(testId).Returns(new ToDoItem());
        // Mock the DeleteById method to do nothing (success scenario)
        repositoryMock.When(r => r.DeleteByIdAsync(testId)).Do(x => { });
        //Act
        var result = await controller.DeleteByIdAsync(testId);
        //Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content
        repositoryMock.Received(1).ReadByIdAsync(testId);
        repositoryMock.Received(1).DeleteByIdAsync(testId);
    }

    [Fact]
    public async Task Delete_InvalidItemId_ReturnsNotFound_404()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int nonExistentId = 999;

        // Mock the ReadById method to return null for a non-existent ID
        repositoryMock.ReadByIdAsync(nonExistentId).Returns((ToDoItem)null); // Simulate that item with ID 999 does not exist

        // Act
        var result = await controller.DeleteByIdAsync(nonExistentId); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
        repositoryMock.Received(1).ReadByIdAsync(nonExistentId); // Ensure ReadById was called with the correct ID
        repositoryMock.DidNotReceive().DeleteByIdAsync(nonExistentId); // Ensure DeleteById was not called
    }
    [Fact]
    public async Task DeleteById_UnhandledException_ReturnsInternalServerError_500()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;

        // Simulate an unhandled exception when trying to delete the item
        repositoryMock.When(r => r.ReadByIdAsync(testId)).Do(x => { throw new Exception("Unexpected error during ReadById"); });

        // Act
        var result = await controller.DeleteByIdAsync(testId);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting a 500 Internal Server Error response
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Verify that ReadById and DeleteById were called
        repositoryMock.Received(1).ReadByIdAsync(testId);
        repositoryMock.DidNotReceive().DeleteByIdAsync(testId);
    }
    [Fact]
    public async Task DeleteAll_ItemsExist_ReturnsNoContent_204()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock that there are items to delete
        repositoryMock.ReadAllAsync().Returns(new List<ToDoItem> { new ToDoItem { ToDoItemId = 1, Name = "Test Item" } });

        // Mock the DeleteAll method to do nothing (the success scenario)
        repositoryMock.When(r => r.DeleteAllAsync()).Do(callInfo => { });

        // Act
        var result = await controller.DeleteAllAsync(); // Call the DeleteAll action

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content
        repositoryMock.Received(1).DeleteAllAsync(); // Ensure DeleteAll was called exactly once
    }

    [Fact]
    public async Task DeleteAll_NoItemsExist_ReturnsNoContent_204()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock that no items exist to delete
        repositoryMock.ReadAllAsync().Returns(new List<ToDoItem>());

        // Act
        var result = await controller.DeleteAllAsync();

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204
        repositoryMock.Received(1).DeleteAllAsync(); // Ensure DeleteAll was called exactly once
    }

    [Fact]
    public async Task DeleteAll_UnhandledException_ReturnsInternalServerError_500()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Simulate an unhandled exception in DeleteAll
        repositoryMock.When(r => r.DeleteAllAsync()).Do(x => { throw new Exception("Unexpected error"); });

        // Act
        var result = await controller.DeleteAllAsync();

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Ensure DeleteAll was called once, even if an exception occurred
        repositoryMock.Received(1).DeleteAllAsync();
    }
}
