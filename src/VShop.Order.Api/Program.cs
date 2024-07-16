using Serilog;
using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.AddServiceDefaults();
builder.AddRabbitMQClient("RabbitMQConnection");
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

app.MapPost("/order/{productId}", async (IConnection connection, [FromRoute] string productId) =>
{
    Log.Information("Order received for product {productId}", productId);
    // using this factory cause errors:
    // var factory = new ConnectionFactory() { HostName = "localhost" };
    // using var connection = factory.CreateConnection();
    using var channel = connection.CreateModel();
    channel.QueueDeclare(queue: "product_queue",
                         durable: false,
                         exclusive: false,
                         autoDelete: false,
                         arguments: null);

    var message = new { ProductId = productId };
    var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

    channel.BasicPublish(exchange: "",
                         routingKey: "product_queue",
                         basicProperties: null,
                         body: body);

    return Results.Ok("Order received and message sent to product_queue");
});

app.Run();
