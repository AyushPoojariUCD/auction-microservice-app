using MongoDB.Entities;
using MongoDB.Driver;
using SearchService;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var app = builder.Build();

// Middleware
app.UseAuthorization();

app.MapControllers();

try
{
    await DbInitializer.initDb(app);
}
catch (Exception e)
{
    Console.WriteLine(e);
}

app.Run();
