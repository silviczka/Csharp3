using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

//record is a structure, which can be serialised, is immutable (cannot be modified after creation) and is data-efficient, our record will have constructor > string Name, etc

public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted, string? Category = null) //id is generated
{   //alternative code in 1 line
    //public ToDoItem ToDomain() => new() { Name = Name, Description = Description, IsCompleted = IsCompleted };
    public ToDoItem ToDomain()
    {
        var newToDoItem = new ToDoItem
        {
            Name = Name,
            Description = Description,
            IsCompleted = IsCompleted,
            Category = Category
        };
        return newToDoItem;
    }
}
