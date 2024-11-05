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
using ToDoList.Persistence.Repositories;
using NSubstitute;


//class where I will write my tests for the Get methods of the ToDoItemsController
public class GetUnitTests
{
    [Fact] // Fact attribute indicaties that this method is a test method
    //the method "get all items - returns all items" is testing whether the controller correctly returns all ToDoItems
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange - here we set up the objects we will use in the test
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItems = new List<ToDoItem>
        {
            new ToDoItem { ToDoItemId = 1, Name = "Item 1", Description = "Description 1", IsCompleted = false },
            new ToDoItem { ToDoItemId = 2, Name = "Item 2", Description = "Description 2", IsCompleted = true }
        };
        //Mock the Read method to return the list of items
        repositoryMock.Read().Returns(toDoItems);

        // Act - here we call method we want to test
        var result = controller.Read();
        var resultResult = result.Result;

        // Assert
        Assert.IsType<OkObjectResult>(resultResult); //expecting 200 ok
        var okResult = resultResult as OkObjectResult;
        var value = okResult.Value as IEnumerable<ToDoItemGetResponseDto>;

        Assert.NotNull(value);
        Assert.Equal(2, value.Count()); // Ensure we get back 2 items

        var firstItem = value.First();
        Assert.Equal(toDoItems[0].ToDoItemId, firstItem.Id);
        Assert.Equal(toDoItems[0].Description, firstItem.Description);
        Assert.Equal(toDoItems[0].IsCompleted, firstItem.IsCompleted);
        Assert.Equal(toDoItems[0].Name, firstItem.Name);
    }

    [Fact]
    public void Get_NoItems_ReturnsNotFound()
    {
        // Arrange - set up the mock repository
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        // Mock the Read method to return an empty list
        repositoryMock.Read().Returns(new List<ToDoItem>());
        // Act
        var result = controller.Read();
        var resultResult = result.Result;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult); //expecting 404 Not Found
    }
}
