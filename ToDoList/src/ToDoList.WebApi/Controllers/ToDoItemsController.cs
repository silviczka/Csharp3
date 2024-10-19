namespace ToDoList.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[ApiController]
[Route("api/[controller]")] //bude se to jmenovat ToDoItems po vzoru nazvu classu
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> items = [];

    [HttpPost]
    //DTO data transfer object
    public IActionResult Create(ToDoItemCreateRequestDto request)
    {
        //map to Domain object as soon as possible
        var item = request.ToDomain();

        //try to create an item
        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(o => o.ToDoItemId) + 1;
            items.Add(item);
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
        }

        //respond to client

        return Created(); // to jsme si s Honzou napsali v nasem reseni, vraci to status code 201 as oppsoed to NoContent , kt.vraci 204, podle AI je spravnejsi Created - 201, protoze zobrazi clientovi informaci
         //return NoContent();    //201 //tato metoda z nějakého důvodu vrací status code No Content 204, zjištujeme proč ;) << error
    }

    [HttpGet]
    public IActionResult Read() //Honzovo reseni implementuje try catch, coz AI schvaluje vice
    {
        try // v try bloku jde o simulaci chyby, aby jsme overili, ze catch blok funguje, v beznem kodu bejsme to nenasli, tam osetrujeme realne zalezitosti
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
    public IActionResult ReadById(int toDoItemId)
    {
        return Ok();
    }

    [HttpPut("{toDoItemId:int}")]
    public IActionResult UpdateById(int toDoItemId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        return Ok();
    }

    [HttpDelete("{toDoItemId:int}")]
    public IActionResult DeleteById(int toDoItemId)
    {
        return Ok();
    }
}
