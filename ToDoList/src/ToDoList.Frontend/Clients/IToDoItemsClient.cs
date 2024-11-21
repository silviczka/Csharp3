using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ToDoList.Frontend.Clients;
using ToDoList.Frontend.Views;
public interface IToDoItemsClient
{
    public Task <List<ToDoItemView>> ReadItemsAsync();
}
