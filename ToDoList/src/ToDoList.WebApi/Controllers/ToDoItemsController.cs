namespace ToDoList.WebApi.Controllers;

using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[ApiController]
[Route("api/[controller]")]   //bude se to jmenovat ToDoItems po vzoru nazvu classu
public class ToDoItemsController : ControllerBase
{

    private static List<ToDoItem> items = [];
    [HttpPost]
    //DTO data transfer object
    public ActionResult Create(ToDoItemCreateRequestDto request)
    {
        try
        {
            var newToDoItem = request.ToDomain();
            var id = items.Count+1;
            newToDoItem.ToDoItemId = id;
            items.Add(newToDoItem);
        }
        catch(Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Created();
    }
    [HttpGet]
    public IActionResult Read()
    {


        try

        {
            throw new Exception("Neco se pokazilo");
        }
        catch (Exception ex)
        {
            this.Problem(ex.Message, null, StatusCodes.Status500InternalServerError);
        }
        return Ok();
    }
    [HttpGet("{toDoItemId:int}")]
    public ActionResult ReadById(int toDoItemId)
    {

        return Ok();
    }
    [HttpPut("{toDoItemId:int}")]
    public ActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok();

    }
    [HttpDelete("{toDoItemId:int}")]
    public ActionResult DeleteById(int toDoItemId)
    {
        return Ok();

    }
}
