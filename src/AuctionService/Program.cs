using AuctionService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))); // ✅ fixed here

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
