namespace ToDoList.WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[ApiController]
[Route("api/[controller]")]
public class ToDoItemsController : ControllerBase
{
    private readonly ToDoItemsContext context; // class throough which we communicate with database
    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;
    }

    /* public ToDoItemsController()
    {
    } */

    [HttpPost]
    //DTO data transfer object
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        //map DTO to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create and add the item to the list
        try
        {
            //generate a new ID for the item and add it to the database

            /*OG code:
/*          item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item); */

            //NEW CODE:  we do not need to set up the rule max +1, the database it doing it itself
            context.ToDoItems.Add(item);
            context.SaveChanges();
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
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read()
    {
        List<ToDoItem> itemsToGet;
        // Try to retrieve the list of items
        try
        {
            itemsToGet = context.ToDoItems.ToList();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        //if the list is null, return 404 not found, else return 200 OK with the List of items mapped to DTOs
        return (itemsToGet is null || !itemsToGet.Any())
            ? NotFound() //404
            : Ok(itemsToGet.Select(ToDoItemGetResponseDto.FromDomain).ToList()); //200
    }

    [HttpGet("{toDoItemId:int}")]
    public ActionResult<ToDoItemGetResponseDto> ReadById(int toDoItemId)
    {
        //try to retrieve the item by its ID
        ToDoItem? itemToGet;
        try
        {
            //use LINQ's Find() to search for the item by ID
            itemToGet = context.ToDoItems.Find(toDoItemId);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        //if item is not found, return 404 Not Found, else return 200 OK with the item converted to DTO
        return (itemToGet is null)
            ? NotFound() //404
            : Ok(ToDoItemGetResponseDto.FromDomain(itemToGet)); //200
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        //map the request DTO to a domain object as soon as possible
        var updatedItem = request.ToDomain();

        //try to update the item by retrieving it with given id
        try
        {
            //find the existing item in the database
            var itemToUpdate = context.ToDoItems.Find(toDoItemId);

            //if the item is not found, return 404 Not Found
            if (itemToUpdate == null)
            {
                return NotFound(); //404
            }
            //update the existing item's properties
            itemToUpdate.Name = updatedItem.Name;
            itemToUpdate.Description = updatedItem.Description;
            itemToUpdate.IsCompleted = updatedItem.IsCompleted;

            context.ToDoItems.Update(itemToUpdate);
            context.SaveChanges();
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
            //use LINQ Find method to locate the item by its ID
            var itemToDelete = context.ToDoItems.Find(toDoItemId);
            //it the item is not found, return 404 Not Found
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            //if the item is found, remove it from the list
            context.ToDoItems.Remove(itemToDelete);
            context.SaveChanges();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client - return 204 No Content to indicate successful deletion
        return NoContent(); //204
    }
}
