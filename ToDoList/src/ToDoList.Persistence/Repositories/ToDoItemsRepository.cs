using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

namespace ToDoList.Persistence.Repositories;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context;
    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task CreateAsync(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task <IEnumerable<ToDoItem>> ReadAllAsync()
    {
        return await context.ToDoItems.ToListAsync();
    }

    public async Task<ToDoItem?> ReadByIdAsync (int id)
    {
        return await context.ToDoItems.FindAsync(id);
    }

    public async Task UpdateAsync(ToDoItem item)
    {
        var existingItem = await context.ToDoItems.FindAsync(item.ToDoItemId);
        //check if the item exists in the database
        if (existingItem == null)
        {
            throw new InvalidOperationException("Item not found for update"); // Throw an exception if the item is not found
        }
        // Update properties if the item exists
        existingItem.Name = item.Name;
        existingItem.Description = item.Description;
        existingItem.IsCompleted = item.IsCompleted;
        existingItem.Category = item.Category;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAllAsync()
    {
        // Delete all items directly in the database without loading them
        int deletedCount = await context.ToDoItems.ExecuteDeleteAsync();

        // handle if no items were deleted
        if (deletedCount == 0)
        {
            Console.WriteLine("No items found for deletion.");
        }
    }

    public async Task DeleteByIdAsync(int id)
    {
        var item = await context.ToDoItems.FindAsync(id);
        if (item == null)
        {
            throw new InvalidOperationException("Item not found for deletion by ID");
        }
        context.ToDoItems.Remove(item);
        await context.SaveChangesAsync();
    }


}
