using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;

namespace ToDoList.Test;

public class DeleteTests
{

[Fact]
    public void Delete_ItemExists_Returns204()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        ToDoItemsController.items.Add(toDoItem);

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        Assert.Empty(ToDoItemsController.items); // Ensure the item was removed
    }

    [Fact]
    public void Delete_ItemDoesNotExist_Returns404()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.DeleteById(999); // Non-existent ID

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
