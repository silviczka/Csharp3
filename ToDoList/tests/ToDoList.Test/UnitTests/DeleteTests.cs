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
    public void Delete_ValidItemId_ReturnsNoContent_204()
    {
        //Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;
        // Mock that item exists
        repositoryMock.ReadById(testId).Returns(new ToDoItem());
        // Mock the DeleteById method to do nothing (success scenario)
        repositoryMock.When(r => r.DeleteById(testId)).Do(x => { });
        //Act
        var result = controller.DeleteById(testId);
        //Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content
        repositoryMock.Received(1).ReadById(testId);
        repositoryMock.Received(1).DeleteById(testId);
    }

    [Fact]
    public void Delete_InvalidItemId_ReturnsNotFound_404()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int nonExistentId = 999;

        // Mock the ReadById method to return null for a non-existent ID
        repositoryMock.ReadById(nonExistentId).Returns((ToDoItem)null); // Simulate that item with ID 999 does not exist

        // Act
        var result = controller.DeleteById(nonExistentId); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
        repositoryMock.Received(1).ReadById(nonExistentId); // Ensure ReadById was called with the correct ID
        repositoryMock.DidNotReceive().DeleteById(nonExistentId); // Ensure DeleteById was not called
    }
    [Fact]
    public void DeleteById_UnhandledException_ReturnsInternalServerError_500()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;

        // Simulate an unhandled exception when trying to delete the item
        repositoryMock.When(r => r.ReadById(testId)).Do(x => { throw new Exception("Unexpected error during ReadById"); });

        // Act
        var result = controller.DeleteById(testId);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting a 500 Internal Server Error response
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Verify that ReadById and DeleteById were called
        repositoryMock.Received(1).ReadById(testId);
        repositoryMock.DidNotReceive().DeleteById(testId);
    }
    [Fact]
    public void DeleteAll_ItemsExist_ReturnsNoContent_204()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock that there are items to delete
        repositoryMock.ReadAll().Returns(new List<ToDoItem> { new ToDoItem { ToDoItemId = 1, Name = "Test Item" } });

        // Mock the DeleteAll method to do nothing (the success scenario)
        repositoryMock.When(r => r.DeleteAll()).Do(callInfo => { });

        // Act
        var result = controller.DeleteAll(); // Call the DeleteAll action

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content
        repositoryMock.Received(1).DeleteAll(); // Ensure DeleteAll was called exactly once
    }

    [Fact]
    public void DeleteAll_NoItemsExist_ReturnsNoContent_204()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock that no items exist to delete
        repositoryMock.ReadAll().Returns(new List<ToDoItem>());

        // Act
        var result = controller.DeleteAll();

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204
        repositoryMock.Received(1).DeleteAll(); // Ensure DeleteAll was called exactly once
    }

    [Fact]
    public void DeleteAll_UnhandledException_ReturnsInternalServerError_500()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Simulate an unhandled exception in DeleteAll
        repositoryMock.When(r => r.DeleteAll()).Do(x => { throw new Exception("Unexpected error"); });

        // Act
        var result = controller.DeleteAll();

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Ensure DeleteAll was called once, even if an exception occurred
        repositoryMock.Received(1).DeleteAll();
    }
}
