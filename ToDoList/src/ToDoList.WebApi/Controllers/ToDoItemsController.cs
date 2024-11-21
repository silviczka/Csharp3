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
    private readonly IRepository<ToDoItem> repository;
    public ToDoItemsController(IRepository<ToDoItem> repository)
    {
        this.repository = repository;
    }


    [HttpPost]
    //DTO data transfer object
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        //map DTO to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create and add the item to the list
        try
        {
            repository.Create(item);
        }
        catch (Exception ex)
        {
            //if there is an error, return a 500 response
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client - return a 201 response and provide a location for the created item
        return CreatedAtAction(
            nameof(ReadById),  //Action that retrieves the item by ID
            new { toDoItemId = item.ToDoItemId },   // Route values for the action
            ToDoItemGetResponseDto.FromDomain(item)); //201 status code - response body with the created item
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> ReadAll()
    {
        try
        {
            var itemsToGet = repository.ReadAll();
            // Explicitly check for null and empty lists
            if (itemsToGet == null || !itemsToGet.Any())
            {
                return NotFound(); // Return NotFound if no items or if the list is null
            }
            return Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        try
        {
            var itemToGet = repository.ReadById(toDoItemId);
            return itemToGet is null ? NotFound() : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet));
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map the request DTO to a domain object as soon as possible
        var updatedItem = request.ToDomain();

        //try to update the item by retrieving it with given id
        try
        {
            //find the existing item in the repository
            var itemToUpdate = repository.ReadById(toDoItemId);

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
            repository.Update(itemToUpdate);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client - if successful, return 204 No Content
        return NoContent(); //204
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        //try to delete the item by finding it first
        try
        {
            var itemToDelete = repository.ReadById(toDoItemId);
            //it the item is not found, return 404 Not Found
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            //if the item is found, remove it from the repository
            repository.DeleteById(toDoItemId);
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
    public IActionResult DeleteAll()
    {
        try
        {
            repository.DeleteAll();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        return NoContent(); // Return 204
    }
}
