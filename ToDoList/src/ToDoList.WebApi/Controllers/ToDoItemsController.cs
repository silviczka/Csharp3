namespace ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[ApiController]
[Route("api/[controller]")] //bude se to jmenovat ToDoItems po vzoru nazvu classu
public class ToDoItemsController : ControllerBase
{
    public static readonly List<ToDoItem> items = [];

    [HttpPost]
    //DTO data transfer object
    public ActionResult<ToDoItemGetResponseDto> Create(ToDoItemCreateRequestDto request)
    {
        //map DTO to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create and add the item to the list
        try
        {
            //generate a new ID for the item
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item);
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
            itemsToGet = items;
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client
        //if the list is null, return 404 not found, else return 200 OK with the List of items mapped to DTOs
        return (itemsToGet is null)
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
            itemToGet = items.Find(i => i.ToDoItemId == toDoItemId);
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
            //find the index of the item to update using LINQ's FindIndex
            var itemIndexToUpdate = items.FindIndex(i => i.ToDoItemId == toDoItemId);
            //if the item is not found, return 404 Not Found
            //FindIndex() works the wway that if it finds the item that matches the condition,
            //it returns the zero based index of the item in the list
            //contrarywise if FindIndex() does not find an item matching the condition, it returns -1
            //an index of -1 is a common convention for indicating "not found", because the list indices typically start at 0
            if (itemIndexToUpdate == -1)
            {
                return NotFound(); //404
            }
            //Update the item;s ID to the given ToDoItemId to ensure the ID stays consistent
            updatedItem.ToDoItemId = toDoItemId;
            //Replace the item at the found index with the updated item
            items[itemIndexToUpdate] = updatedItem;
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
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
            var itemToDelete = items.Find(i => i.ToDoItemId == toDoItemId);
            //it the item is not found, return 404 Not Found
            if (itemToDelete is null)
            {
                return NotFound(); //404
            }
            //if the item is found, remove it from the list
            items.Remove(itemToDelete);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }

        //respond to client - return 204 No Content to indicate successful deletion
        return NoContent(); //204
    }
}
