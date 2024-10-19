using System;
using ToDoList.Domain.Models;

namespace ToDoList.Domain.DTOs;

//record je struktura, ktera se dokaze serializovat a je setrna ohledne dat, a tento record bude mit konstruktor > string Name, etc

public record ToDoItemCreateRequestDto(string Name, string Description, bool isCompleted) //id is generated
{   //alternativny zapis v 1 riadku
    //public ToDoItem ToDomain() => new() { Name = Name, Description = Description, IsCompleted = IsCompleted };
    public ToDoItem ToDomain()
    {
        var newToDoItem = new ToDoItem
        {
            Name = Name,
            Description = Description,
            IsCompleted = isCompleted
        };
        return newToDoItem;
    }
}
