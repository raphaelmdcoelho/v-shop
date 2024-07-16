# V-SHOP

## Description

## Implementation steps

### Git repository configuration
* git init
* git branch -M main
* git push origin main

### Create dotnet solution
* dotnet new sln --name VShop

### Create Order API and add it to the solution
* dotnet new web --name VShop.Order.Api
* dotnet sln add  .\VShop.Order.Api\VShop.Order.Api.csproj

### Create Inventory API and add it to the solution
* dotnet new web --name VShop.Inventory.Api
* dotnet sln add .\VShop.Inventory.Api/VShop.Inventory.Api.csproj

### Create and configurate App Host Aspire project
* dotnet new aspire-apphost -o eVShop.AppHost
* dotnet sln add .\VShop.AppHost\VShop.AppHost.csproj   
* dotnet add .\VShop.AppHost\VShop.AppHost.csproj reference .\VShop.Order.Api\VShop.Order.Api.csproj
* dotnet add .\VShop.AppHost\VShop.AppHost.csproj reference .\VShop.Inventory.Api/VShop.Inventory.Api.csproj

### Create Aspire Service defaults
* dotnet new aspire-servicedefaults --name VShop.ServiceDefaults
* dotnet sln .\VShop.sln add .\VShop.ServiceDefaults\VShop.ServiceDefaults.csproj
* dotnet add .\VShop.AppHost\VShop.AppHost.csproj reference .\VShop.ServiceDefaults\VShop.ServiceDefaults.csproj
* dotnet add .\VShop.Order.Api\VShop.Order.Api.csproj reference .\VShop.ServiceDefaults\VShop.ServiceDefaults.csproj
* dotnet add .\VShop.Inventory.Api/VShop.Inventory.Api.csproj reference .\VShop.ServiceDefaults\VShop.ServiceDefaults.csproj

### Add rabbit mq Aspire dependencies
* dotnet add package Aspire.RabbitMQ.Client


### Add Serilog

* dotnet add package Serilog
* dotnet add package Serilog.Sinks.Console

### Start Aspire

* dotnet run --project ./VShop.AppHost/VShop.AppHost.csproj

## How to run

## Dependencies

* microsoft.extensions.configuration
* microsoft.extensinos.dependencyinjection
* microsoft.entityframeworkcore.sqlite
* microsoft.extensions.configuration.json
* microsoft.extensions.configuration.file
* microsoft.entityframeworkcore.design

## Migrations

* dotnet ef migrations add Initial --context InventoryDbContext
* dotnet ef database update