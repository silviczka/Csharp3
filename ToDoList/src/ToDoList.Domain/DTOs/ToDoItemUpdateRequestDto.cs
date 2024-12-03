using System;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public record class ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted, string? Category = null)
{

    //convert the DTO to a domain model, we use lambda expression instead of return prompt which makes code shorter/cleaner
    public ToDoItem ToDomain() => new() { Name = this.Name, Description = this.Description, IsCompleted = this.IsCompleted, Category = this.Category};

    //alternative code:
    /*  return new ToDoItem
        {
            Name = this.Name,
            Description = this.Description,
            IsCompleted = this.IsCompleted
        };
    */
}
