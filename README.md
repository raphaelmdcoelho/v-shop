# V-SHOP

## Implementation steps

* git init
* git branch -M main
* git push origin main

* dotnet add sln --name VShop

* dotnet new web --name VShop.Order.Api
* dotnet sln add  .\VShop.Order.Api\VShop.Order.Api.csproj

* dotnet new web --name VShop.Inventory.Api
* dotnet sln add VShop.Inventory.Api/VShop.Inventory.Api.csproj

## How to run
