using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<InventoryDbContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

//TODO: create a extension file to store the actions
//TODO: create a test to get product method
app.MapGet("/", ([FromServices] InventoryDbContext dbContext) => {
    return dbContext.Products.ToList();
});

app.Run();
