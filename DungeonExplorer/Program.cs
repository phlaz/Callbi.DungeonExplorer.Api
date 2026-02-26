using DungeonExplorer.Api.Domain;
using DungeonExplorer.Api.Service;
using DungeonExplorer.Api.Storage;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DungeonContext>(opt => opt.UseSqlite("Data Source=dungeon.db"));
builder.Services.AddScoped<IDungeonRepository, DungeonRepository>();
builder.Services.AddScoped<IPathfindingService, PathfindingService>();
builder.Services.AddScoped<IDungeonService, DungeonService>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();