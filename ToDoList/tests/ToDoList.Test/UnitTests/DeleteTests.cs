using System;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test;

public class DeleteUnitTests
{

    [Fact]
    public void Delete_ItemExists_Returns204()
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

        // Mock the Delete method to simulate a successful deletion
        repositoryMock.ReadById(toDoItem.ToDoItemId).Returns(toDoItem); // Simulate that the item exists
        repositoryMock.Delete(toDoItem.ToDoItemId).Returns(true); // Simulate successful deletion

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content
    }

    [Fact]
    public void Delete_ItemDoesNotExist_Returns404()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock the ReadById method to return null for a non-existent ID
        repositoryMock.ReadById(999).Returns((ToDoItem)null); // Simulate that item with ID 999 does not exist

        // Act
        var result = controller.DeleteById(999); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
    }
}
