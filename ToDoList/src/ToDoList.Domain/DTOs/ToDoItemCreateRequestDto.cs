using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

//record je struktura, ktera se dokaze serializovat a je setrna ohledne dat, a tento record bude mit konstruktor > string Name, etc
public record ToDoItemCreateRequestDto(string Name, string Description, bool isCompleted)
{
public ToDoItem ToDomain()
    {
        var newToDoItem= new ToDoItem
        {
            Name = Name,
            Description = Description,
            isCompleted = isCompleted
        };
        return newToDoItem;

//ToDoItem newToDoItem = new ToDoItem();
//newToDoItem.Name = this.Name;
//newToDoItem.Description = this.Description;
//newToDoItem.isCompleted = this.isCompleted;


    }
}
