namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using ToDoList.Persistence;
using ToDoList.Domain.DTOs;
using System.Collections.Generic;
using System.Linq;
using Xunit;


//class where I will write my tests for the Get methods of the ToDoItemsController
public class GetTests
{
    [Fact] // Fact attribute indicaties that this method is a test method
    //the method "get all items - returns all items" is testing whether the controller correctly returns all ToDoItems
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange - here we set up the objects we will use in the test
        var controller = new ToDoItemsController();
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Jmeno",
            Description = "Popis",
            IsCompleted = false
        };
        //temporarily commented out as database set up is not complete
        //controller.items.Add(toDoItem);

        // Act - here we call method we want to test
        var result = controller.Read();
        var resultResult = result.Result;
        var value = result.GetValue();

        // Assert
        Assert.IsType<OkObjectResult>(resultResult);
        Assert.NotNull(value);

        var firstItem = value.First();
        Assert.Equal(toDoItem.ToDoItemId, firstItem.Id);
        Assert.Equal(toDoItem.Description, firstItem.Description);
        Assert.Equal(toDoItem.IsCompleted, firstItem.IsCompleted);
        Assert.Equal(toDoItem.Name, firstItem.Name);
    }

    [Fact]
    public void Get_NoItems_ReturnsNotFound()
    {
        // Arrange
        var controller = new ToDoItemsController();

        // Act
        var result = controller.Read();
        var resultResult = result.Result;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult);
    }
}
