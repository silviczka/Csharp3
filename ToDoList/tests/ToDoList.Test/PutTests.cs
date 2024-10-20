using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using Xunit;

namespace ToDoList.Test;

public class PutTests
{
    [Fact]
    public void Put_ValidItem_Returns204NoContent()
    {
        // Arrange - we create item called "old" and add it on the list in order to be updated in part Act
        var controller = new ToDoItemsController();
        ToDoItemsController.items = new List<ToDoItem>(); //makes sure the static list is empty before test
        var toDoItem = new ToDoItem { ToDoItemId = 1, Name = "Old Name", Description = "Old Description", IsCompleted = false };
        ToDoItemsController.items.Add(toDoItem);

        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);

        // Act
        var result = controller.UpdateById(1, updateDto); // Call the method to update the item on position 1 with parameters of updateDto

        // Assert
        Assert.IsType<NoContentResult>(result); // Expecting 204 No Content

        // Verify the item was updated
        var updatedItem = ToDoItemsController.items.First(i => i.ToDoItemId == 1);
        Assert.Equal(updateDto.Name, updatedItem.Name);
        Assert.Equal(updateDto.Description, updatedItem.Description);
        Assert.Equal(updateDto.IsCompleted, updatedItem.IsCompleted);
    }

    [Fact]
    public void Put_ItemNotFound_Returns404NotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);

        // Act
        var result = controller.UpdateById(999, updateDto); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result); // Expecting 404 Not Found
    }

    [Fact]
    public void Put_Exception_Returns500InternalServerError()
    {
        // Arrange
        var controller = new ToDoItemsController();
        // Simulating an error by setting items list to null or an invalid state
        ToDoItemsController.items = null;

        var updateDto = new ToDoItemUpdateRequestDto("Updated Name", "Updated Description", true);

        // Act
        var result = controller.UpdateById(1, updateDto);

        // Assert
        Assert.IsType<ObjectResult>(result); // Expecting 500 Internal Server Error
        var objectResult = result as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);
    }
}
