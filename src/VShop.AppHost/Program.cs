var builder = DistributedApplication.CreateBuilder(args);

var order = builder.AddProject<Projects.VShop_Order_Api>("order")
    .WithExternalHttpEndpoints();
    // .WithReference(inventory);

var inventory = builder.AddProject<Projects.VShop_Inventory_Api>("inventory")
    .WithExternalHttpEndpoints()
    .WithReference(order);

builder.Build().Run();
