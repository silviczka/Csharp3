using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using Xunit;

namespace ToDoList.Test;

public class PutUnitTests
{
    [Fact]
    public void Put_ValidItem_Returns204NoContent()
    {
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItem = new ToDoItem { ToDoItemId = 1, Name = "Old Name", Description = "Old Description", IsCompleted = false };

        // Configure repository to return the existing item when ReadById is called
        repositoryMock.ReadById(1).Returns(toDoItem);
        // Configure the repository to update the item and indicate success
        repositoryMock.Update(Arg.Any<ToDoItem>()).Returns(true);


        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);

        // Act
        var result = controller.UpdateById(1, updateDto); // Call the method to update the item on position 1 with parameters of updateDto

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content

        // Verify that the Update method was called with the correct parameters
            repositoryMock.Received(1).Update(Arg.Is<ToDoItem>(
            item => item.ToDoItemId == 1 &&
            item.Name == updateDto.Name &&
            item.Description == updateDto.Description &&
            item.IsCompleted == updateDto.IsCompleted));
    }

    [Fact]
    public void Put_ItemNotFound_Returns404NotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);
        //configure the repository to return null for non-existent ID
        repositoryMock.ReadById(999).Returns((ToDoItem)null);

        // Act
        var result = controller.UpdateById(999, updateDto); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
    }

    [Fact]
    public void Put_Exception_Returns500InternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);
        //configure the repository to throw an exception when trying to update
        repositoryMock.When(r => r.Update(Arg.Any<ToDoItem>())).Do(x => { throw new Exception(); });
        repositoryMock.ReadById(1).Returns(new ToDoItem { ToDoItemId = 1 }); // Simulating an existing item

        // Act
        var result = controller.UpdateById(1, updateDto);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500 Internal Server Error
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);
    }
}
