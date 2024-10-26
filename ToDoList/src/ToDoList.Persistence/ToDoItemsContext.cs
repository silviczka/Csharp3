namespace ToDoList.Persistence;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContext : DbContext
{
    private readonly string connectionString;
    public ToDoItemsContext(string connectionString= "Data Source=../../data/localdb.db")
    {
        this.connectionString = connectionString;
        this.Database.Migrate(); //migration is conversion from one state of table to new state
        //in our migrations folder we have initial create.cs file , in this file we have mehtods Up and Down which let us upgrade or downgrade versions
    }
    public DbSet<ToDoItem> ToDoItems{get;set;}
    //we must mass project refference from .Persistence to Domain where ToDoItem model is created
    //then I must highlight ToDoItems which is still red underlined and with CTR+. I select using ToDoList.Domain.Models;
    //same we must add reference from ToDoList.webapi to ToDoList.Persistence
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // we want override (= pretizit) method OnConfiguring
    {
        optionsBuilder.UseSqlite(connectionString);// chceme se pripojit k databazi na te ceste "Data Source=....
    }
}
