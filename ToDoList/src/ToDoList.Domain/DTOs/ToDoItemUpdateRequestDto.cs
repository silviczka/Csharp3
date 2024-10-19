using System;
using System.ComponentModel;
using Microsoft.VisualBasic;

namespace ToDoList.Domain.DTOs;

public record ToDoItemUpdateRequestDto(string Name, string Description, bool IsCompleted)
{
    
}
