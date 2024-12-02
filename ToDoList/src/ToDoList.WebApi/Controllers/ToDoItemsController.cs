namespace ToDoList.WebApi.Controllers;

using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repository;
    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository)
    {
        this.repository = repository;
    }


    [HttpPost]
    //DTO data transfer object
    public async Task <ActionResult<ToDoItemGetResponseDto>> CreateAsync(ToDoItemCreateRequestDto request)
    {
        //map DTO to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create and add the item to the list
        try
        {
            repository.CreateAsync(item);
        }
        catch (Exception ex)
        {
            //if there is an error, return a 500 response
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client - return a 201 response and provide a location for the created item
        return CreatedAtAction(
            nameof(ReadByIdAsync),  //Action that retrieves the item by ID
            new { toDoItemId = item.ToDoItemId },   // Route values for the action
            ToDoItemGetResponseDto.FromDomain(item)); //201 status code - response body with the created item
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> ReadAllAsync()
    {
        try
        {
            var itemsToGet = await repository.ReadAllAsync();
            // Explicitly check for null and empty lists
            if (itemsToGet == null || !itemsToGet.Any())
            {
                return NotFound(); // Return NotFound if no items or if the list is null
            }
            return Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadByIdAsync(int toDoItemId)
    {
        try
        {
            var itemToGet = await repository.ReadByIdAsync(toDoItemId);
            return itemToGet is null ? NotFound() : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public async Task <IActionResult> UpdateByIdAsync(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map the request DTO to a domain object as soon as possible
        var updatedItem = request.ToDomain();

        //try to update the item by retrieving it with given id
        try
        {
            //find the existing item in the repository
            var itemToUpdate = await repository.ReadByIdAsync(toDoItemId);

            //if the item is not found, return 404 Not Found
            if (itemToUpdate == null)
            {
                return NotFound(); //404
            }
            //update the existing item's properties
            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.IsCompleted = updatedItem.IsCompleted;
            itemToUpdate.Category = updatedItem.Category;
            await repository.UpdateAsync(itemToUpdate);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client - if successful, return 204 No Content
        return NoContent(); //204
    }

    [HttpDelete("{toDoItemId:int}")]
    public async Task<IActionResult> DeleteByIdAsync(int toDoItemId)
    {
        //try to delete the item by finding it first
        try
        {
            var itemToDelete = await repository.ReadByIdAsync(toDoItemId);
            //it the item is not found, return 404 Not Found
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            //if the item is found, remove it from the repository
            await repository.DeleteByIdAsync(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client - return 204 No Content to indicate successful deletion
        return NoContent(); //204
    }

    [HttpDelete]
    [Route("all")]
    public async Task<IActionResult> DeleteAllAsync()
    {
        try
        {
            await repository.DeleteAllAsync();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent(); // Return 204
    }
}
