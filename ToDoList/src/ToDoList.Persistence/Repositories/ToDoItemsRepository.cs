using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context;
    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public void Create(ToDoItem item)
    {
            context.ToDoItems.Add(item);
            context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read()
    {
        return context.ToDoItems.ToList();
    }

    public ToDoItem? ReadById(int id)
    {
        return context.ToDoItems.Find(id);
    }

    public bool Update(ToDoItem item)
    {
        var existingItem = context.ToDoItems.Find(item.ToDoItemId);
        //check if the item exists in the database
        if (existingItem == null)
        {
            return false; //item not found, update failed
        }
        // Update properties if the item exists
        existingItem.Name = item.Name;
        existingItem.Description = item.Description;
        existingItem.IsCompleted = item.IsCompleted;
        context.ToDoItems.Update(item);
        context.SaveChanges();
        return true; // update successful
    }

    public bool Delete(int id)
    {
        var item = context.ToDoItems.Find(id);
        if (item != null)
        {
            context.ToDoItems.Remove(item);
            context.SaveChanges();
        }
        return true;
    }
}
