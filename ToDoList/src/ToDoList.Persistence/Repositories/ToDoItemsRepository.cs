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

    public IEnumerable<ToDoItem> ReadAll()
    {
        return context.ToDoItems.ToList();
    }

    public ToDoItem? ReadById(int id)
    {
        return context.ToDoItems.Find(id);
    }

    public void Update(ToDoItem item)
    {
        var existingItem = context.ToDoItems.Find(item.ToDoItemId);
        //check if the item exists in the database
        if (existingItem == null)
        {
            throw new InvalidOperationException("Item not found for update"); // Throw an exception if the item is not found
        }
        // Update properties if the item exists
        existingItem.Name = item.Name;
        existingItem.Description = item.Description;
        existingItem.IsCompleted = item.IsCompleted;
        context.SaveChanges();
    }

    public void DeleteAll()
    {
        var allItems = context.ToDoItems.ToList(); // Load all items
        if (!allItems.Any())
        {
            throw new InvalidOperationException("No items found for deletion."); // No items to delete
        }
        context.ToDoItems.RemoveRange(allItems);
        context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var item = context.ToDoItems.Find(id);
        if (item == null)
        {
            throw new InvalidOperationException("Item not found for deletion by ID"); // Throw exception if not found
        }
        context.ToDoItems.Remove(item);
        context.SaveChanges();
    }


}
