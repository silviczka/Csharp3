using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

namespace ToDoList.Frontend.Views
{
   public class ToDoItemView
    {
        public int ToDoItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public string? Category {get; set;}
    }
}
