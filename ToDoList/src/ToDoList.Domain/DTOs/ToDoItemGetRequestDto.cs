namespace ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public record class ToDoItemGetResponseDto(int Id, string Name, string Description, bool IsCompleted, string? Category) //let client know the Id
{
    // Method to convert domain model to DTO
    public static ToDoItemGetResponseDto FromDomain(ToDoItem item)
    {

        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "ToDoItem cannot be null");
        }

        return new ToDoItemGetResponseDto(
                item.ToDoItemId,
                item.Name,
                item.Description,
                item.IsCompleted,
                item.Category
        );
    }
}
