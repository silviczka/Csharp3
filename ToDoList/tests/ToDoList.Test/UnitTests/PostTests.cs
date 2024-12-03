using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.WebApi.Controllers;
using Xunit;
using NSubstitute;
using ToDoList.Persistence.Repositories;
using NSubstitute.Routing.Handlers;

namespace ToDoList.Test;

public class PostUnitTests
{
    [Fact]
    public async Task Post_CreateValidRequest_HasCategory_ReturnsCreatedAtAction()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var requestDto = new ToDoItemCreateRequestDto("Test Name", "Test Description", false, "something"); //we expect the new ToDoItem will not be completed at the time of creation
        var createdItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = requestDto.Name,
            Description = requestDto.Description,
            IsCompleted = requestDto.IsCompleted,
            Category = requestDto.Category
        };
        // Configure repository mock to simulate the successful creation
        repositoryMock.When(r => r.CreateAsync(Arg.Any<ToDoItem>())).Do(x => { });
        repositoryMock.ReadByIdAsync(Arg.Is<int>(id => id == 1)).Returns(createdItem);

        // Act
        var result = await controller.CreateAsync(requestDto);
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        var value = createdAtActionResult?.Value as ToDoItemGetResponseDto;

        // Assert
        Assert.IsType<CreatedAtActionResult>(createdAtActionResult); // Expecting 201 Created
        Assert.NotNull(value);

        // Verifying that the returned values are correct
        Assert.Equal(requestDto.Name, value.Name);
        Assert.Equal(requestDto.Description, value.Description);
        Assert.Equal(requestDto.IsCompleted, value.IsCompleted);
        Assert.Equal(requestDto.Category, value.Category);

        // Verify that the Create method was called once with a ToDoItem that matches the DTO properties
        repositoryMock.Received(1).CreateAsync(Arg.Is<ToDoItem>(
            item => item.Name == requestDto.Name &&
                    item.Description == requestDto.Description &&
                    item.IsCompleted == requestDto.IsCompleted &&
                    item.Category == requestDto.Category));
    }

    [Fact]
    public async Task Post_CreateValidRequest_NullCategory_ReturnsCreatedAtAction()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        var requestDto = new ToDoItemCreateRequestDto("Test Name", "Test Description", false); //we expect the new ToDoItem will not be completed at the time of creation
        var createdItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = requestDto.Name,
            Description = requestDto.Description,
            IsCompleted = requestDto.IsCompleted,
            Category = requestDto.Category
        };
        // Configure repository mock to simulate the successful creation
        repositoryMock.When(r => r.CreateAsync(Arg.Any<ToDoItem>())).Do(x => { });
        repositoryMock.ReadByIdAsync(Arg.Is<int>(id => id == 1)).Returns(createdItem);

        // Act
        var result = await controller.CreateAsync(requestDto);
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        var value = createdAtActionResult?.Value as ToDoItemGetResponseDto;

        // Assert
        Assert.IsType<CreatedAtActionResult>(createdAtActionResult); // Expecting 201 Created
        Assert.NotNull(value);

        // Verifying that the returned values are correct
        Assert.Equal(requestDto.Name, value.Name);
        Assert.Equal(requestDto.Description, value.Description);
        Assert.Equal(requestDto.IsCompleted, value.IsCompleted);
        Assert.Null(value.Category);

        // Verify that the Create method was called once with a ToDoItem that matches the DTO properties
        repositoryMock.Received(1).CreateAsync(Arg.Is<ToDoItem>(
            item => item.Name == requestDto.Name &&
                    item.Description == requestDto.Description &&
                    item.IsCompleted == requestDto.IsCompleted &&
                    item.Category == requestDto.Category && item.Category == null));
    }

    [Fact]
    public async Task Post_CreateUnhandledException_Returns500InternalServerError()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var requestDto = new ToDoItemCreateRequestDto("Test Name", "Test Description", false);

        // Configure repository mock to throw an exception, simulating a 500 error
        repositoryMock.When(r => r.CreateAsync(Arg.Any<ToDoItem>())).Do(r => throw new Exception());

        // Act
        var result = await controller.CreateAsync(requestDto);

        // Assert
        Assert.IsType<ObjectResult>(result.Result); // Expecting 500 Internal Server Error
        var objectResult = result.Result as ObjectResult;
        Assert.Equal(500, objectResult.StatusCode);
    }
}
