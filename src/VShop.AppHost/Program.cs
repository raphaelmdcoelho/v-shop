var builder = DistributedApplication.CreateBuilder(args);

var messaging = builder.AddRabbitMQ("store");

var inventory = builder.AddProject<Projects.VShop_Inventory_Api>("inventory")
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.VShop_Order_Api>("order")
    .WithExternalHttpEndpoints()
    .WithReference(messaging)
    .WithReference(inventory);

builder.Build().Run();
