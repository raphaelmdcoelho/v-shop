using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("RabbitMQConnection");

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Services.AddDbContext<InventoryDbContext>(opt => {
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Do not use that, because this can create a copy of a singleton container:
//var connection = builder.Services.BuildServiceProvider().GetService<IConnection>();

var app = builder.Build();

//TODO: create a extension file to store the actions
//TODO: create a test to get product method
app.MapGet("/", ([FromServices] InventoryDbContext dbContext) => {
    Log.Information("Retrieving products from database");
    return dbContext.Products.ToList();
});

Log.Information("App running...");

app.Run();
