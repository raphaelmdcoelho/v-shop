using System.Net.Sockets;
using k8s.Models;

var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", secret: true);
var password = builder.AddParameter("password", secret: true);

var messaging = builder.AddRabbitMQ("store", username, password, port: 5672)
    .WithExternalHttpEndpoints()
    .WithImageTag("3-management")
    .WithEndpoint(scheme: "http", port: 15672, isProxied: false);

var inventory = builder.AddProject<Projects.VShop_Inventory_Api>("inventory")
    .WithExternalHttpEndpoints()
    .WithReference(messaging);

builder.AddProject<Projects.VShop_Order_Api>("order")
    .WithExternalHttpEndpoints()
    .WithReference(messaging)
    .WithReference(inventory);

builder.Build().Run();
