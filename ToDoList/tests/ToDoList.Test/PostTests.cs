using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using Xunit;

namespace ToDoList.Test;

public class PostTests
{
    [Fact]
    public void Post_ValidItem_ReturnsCreatedItem()
    {
        // Arrange
        var controller = new ToDoItemsController();
        ToDoItemsController.items = new List<ToDoItem>(); //makes sure the static list is empty before test
        var requestDto = new ToDoItemCreateRequestDto("Test Name", "Test Description", false); //we expect the new ToDoItem will not be completed at the time of creation

        // Act
        var result = controller.Create(requestDto);
        var resultResult = result.Result;
        var value = result.GetValue();

        // Assert
        Assert.IsType<CreatedAtActionResult>(resultResult); // Expecting 201 Created
        Assert.NotNull(value);

        // Verifying that the returned values are correct
        Assert.Equal(requestDto.Name, value.Name);
        Assert.Equal(requestDto.Description, value.Description);
        Assert.False(value.IsCompleted);
    }

    [Fact]
    public void Post_Exception_Returns500InternalServerError()
    {
        // Arrange
        var controller = new ToDoItemsController();
        // Simulating an error by setting the items list to null or an invalid state
        ToDoItemsController.items = null;

        var requestDto = new ToDoItemCreateRequestDto("Test Name", "Test Description", false);

        // Act
        var result = controller.Create(requestDto);

        // Assert
        Assert.IsType<ObjectResult>(result.Result); // Expecting 500 Internal Server Error
        var objectResult = result.Result as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);
    }
}
