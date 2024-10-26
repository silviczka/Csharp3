namespace ToDoList.Domain.Models;
using System.ComponentModel.DataAnnotations;


public class ToDoItem
{
    [Key] // ToDoItemId is key now
    public int ToDoItemId { get; set; } // ef core looks for field <id> or <nameId>
    [Length(1,50)] //characters 1 to max 50 for Name
    public string Name { get; set; }
    [StringLength(250)]    
    public string Description { get; set; }
    public bool IsCompleted { get; set; }

}
