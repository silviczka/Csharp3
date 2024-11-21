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
using ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using NSubstitute.ExceptionExtensions;


//class where I will write my tests for the Get methods of the ToDoItemsController
public class GetUnitTests
{
    [Fact]
    public void Get_ReadWhenSomeItemIsAvailable_ReturnsOk()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadAll().Returns(new List<ToDoItem>
        {

                new ToDoItem
                {
                    Name = "testName1",
                    Description = "testDescription",
                    IsCompleted = false,
                    Category = "something"
                },
                new ToDoItem
                {
                    Name = "testName2",
                    Description = "testDescription",
                    IsCompleted = false,
                    Category = null
                }
        });
        // Act
        var result = controller.ReadAll();
        var resultResult = result.Result;
        // Assert
        Assert.IsType<OkObjectResult>(resultResult);
        var okResult = resultResult as OkObjectResult;
        var value = okResult.Value as IEnumerable<ToDoItemGetResponseDto>;

        Assert.NotNull(value);
        Assert.Equal(2,value.Count());
        repositoryMock.Received(1).ReadAll();
    }

    [Fact]
    public void Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadAll().Returns(null as IEnumerable<ToDoItem>);
        // Act
        var result = controller.ReadAll();
        var resultResult = result.Result;
        // Assert
        Assert.IsType<NotFoundResult>(resultResult);
        repositoryMock.Received(1).ReadAll();
    }

    [Fact]
    public void Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadAll().Throws(new Exception());
        // Act
        var result = controller.ReadAll();
        var resultResult = result.Result;
        // Assert
        Assert.IsType<ObjectResult>(resultResult);
        repositoryMock.Received(1).ReadAll();
        var objectResult = resultResult as ObjectResult;
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
    }

    [Fact]
    //the method "get all items - returns all items" is testing whether the controller correctly returns all ToDoItems
    public void Get_AllItems_ReturnsAllItems()
    {
        // Arrange - here we set up the objects we will use in the test
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var toDoItems = new List<ToDoItem>
        {
            new ToDoItem { ToDoItemId = 1, Name = "Item 1", Description = "Description 1", IsCompleted = false, Category = null },
            new ToDoItem { ToDoItemId = 2, Name = "Item 2", Description = "Description 2", IsCompleted = true, Category = "important" }
        };
        //Mock the Read method to return the list of items
        repositoryMock.ReadAll().Returns(toDoItems);

        // Act - here we call method we want to test
        var result = controller.ReadAll();
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
        Assert.Equal(toDoItems[0].Category, firstItem.Category);
        Assert.Null(firstItem.Category);
        
        var secondItem = value.Last();
        Assert.Equal(toDoItems[1].ToDoItemId, secondItem.Id);
        Assert.Equal(toDoItems[1].Description, secondItem.Description);
        Assert.Equal(toDoItems[1].IsCompleted, secondItem.IsCompleted);
        Assert.Equal(toDoItems[1].Name, secondItem.Name);
        Assert.Equal(toDoItems[1].Category, secondItem.Category);
    }
}
