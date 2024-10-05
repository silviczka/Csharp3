
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();


//app.MapGet("/", () => "https://api.thecatapi.com/v1/images/search?limit=10");
app.MapGet("/", () => "[{\"id\":\"c1o\",\"url\":\"https://cdn2.thecatapi.com/images/c1o.jpg\",\"width\":555,\"height\":780}]");
app.Run();

