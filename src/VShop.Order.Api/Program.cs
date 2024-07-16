using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

builder.AddServiceDefaults();

var app = builder.Build();

app.MapGet("/", () => {
    Log.Information("Get order");
    return Results.Ok("Order API running...");
});

app.Run();
