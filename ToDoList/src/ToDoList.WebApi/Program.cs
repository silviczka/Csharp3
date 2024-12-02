using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;
using ToDoList.Frontend;

var builder = WebApplication.CreateBuilder(args);
{
    //Configure DI
    //WebApi services
    builder.Services.AddControllers();  //adds ToDoItemsController
    builder.Services.AddSwaggerGen();
    //Persistence Services
    builder.Services.AddDbContext<ToDoItemsContext>(); //adds ToDoItemsContext
    builder.Services.AddScoped<IRepositoryAsync<ToDoItem>,ToDoItemsRepository>();  //adds ToDoItemsRepository
}
var app = builder.Build();
{
    //Configure Middleware (HTTP request pipeline)
    app.MapControllers();
    app.UseSwagger();
    app.UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "ToDoList API V1"));
}


app.Run();
