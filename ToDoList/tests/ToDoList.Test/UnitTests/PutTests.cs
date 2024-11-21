using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace ToDoList.Test;

public class PutUnitTests
{
    [Fact]
    public void Put_UpdateByIdWhenItemUpdated_CategoryProvided_Returns204NoContent()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;
        var existingToDoItem = new ToDoItem { ToDoItemId = testId, Name = "Old Name", Description = "Old Description", IsCompleted = false };

        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true, "something");

        repositoryMock.ReadById(testId).Returns(existingToDoItem);
        repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(x => { }); // Simulate successful update

        // Act
        var result = controller.UpdateById(testId, updateDto); // Call the method to update the item on position 1 with parameters of updateDto

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content

        // Verify that the Update method was called with expected values
        repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
            item.ToDoItemId == testId &&
            item.Name == updateDto.Name &&
            item.Description == updateDto.Description &&
            item.IsCompleted == updateDto.IsCompleted &&
            item.Category == updateDto.Category));
    }

    [Fact]
    public void Put_UpdateByIdWhenItemUpdated_CategorySetToNull_Returns204NoContent()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        int testId = 1;
        var existingToDoItem = new ToDoItem { ToDoItemId = testId, Name = "Old Name", Description = "Old Description", IsCompleted = false, Category = "something" };

        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);

        repositoryMock.ReadById(testId).Returns(existingToDoItem);
        repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(x => { }); // Simulate successful update

        // Act
        var result = controller.UpdateById(testId, updateDto); // Call the method to update the item on position 1 with parameters of updateDto

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content

        // Verify that the Update method was called with expected values
        repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(item =>
            item.ToDoItemId == testId &&
            item.Name == updateDto.Name &&
            item.Description == updateDto.Description &&
            item.IsCompleted == updateDto.IsCompleted &&
            item.Category == null));
    }

    [Fact]
    public void Put_UpdateByIdWhenIdNotFound_Returns404NotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);
        int nonExistentId = 999;
        //configure the repository to return null for non-existent ID
        repositoryMock.ReadById(nonExistentId).Returns((ToDoItem)null);

        // Act
        var result = controller.UpdateById(nonExistentId, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
        repositoryMock.Received(1).ReadById(nonExistentId);
        repositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateById_ReadByIdException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);
        int testId = 1;

        // Set up ReadById to throw an exception
        repositoryMock.When(r => r.ReadById(testId))
                    .Do(x => { throw new Exception("Unhandled exception in ReadById"); });

        // Act
        var result = controller.UpdateById(testId, updateDto);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500 Internal Server Error
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Verify that ReadById was called once, Update was not called due to the exception in ReadById
        repositoryMock.Received(1).ReadById(testId);
        repositoryMock.DidNotReceive().Update(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Put_UpdateById_UpdateException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);
        int testId = 1;

        // Set up ReadById to return an item, simulating a successful retrieval
        repositoryMock.ReadById(testId).Returns(new ToDoItem { ToDoItemId = testId, Name = "Old Name", Description = "Old Description", IsCompleted = false });

        // Set up Update to throw an exception
        repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>()))
                    .Do(x => { throw new Exception("Unhandled exception in Update"); });

        // Act
        var result = controller.UpdateById(testId, updateDto);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500 Internal Server Error
        var objectResult = result as ObjectResult;
        Assert.NotNull(objectResult);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);

        // Verify that ReadById and Update were each called once
        repositoryMock.Received(1).ReadById(testId);
        repositoryMock.Received(1).Update(Arg.Any<ToDoItem>());
    }

}
