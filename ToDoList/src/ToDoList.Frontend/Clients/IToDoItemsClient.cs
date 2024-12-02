using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace ToDoList.Frontend.Clients;
using ToDoList.Frontend.Views;
public interface IToDoItemsClient
{
    public Task <List<ToDoItemView>> ReadItemsAsync();
    Task<ToDoItemView?> ReadItemByIdAsync(int itemId);
    Task CreateItemAsync(ToDoItemView itemView);
    Task UpdateItemAsync(ToDoItemView itemView);
    Task DeleteItemAsync(ToDoItemView itemView);
}
