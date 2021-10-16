# Taledynamic.Api

## Installation proccess without Docker

### 1. Install dotnet-sdk

https://dotnet.microsoft.com/download/dotnet/5.0

### 2. Install dotnet ef as a tool

https://docs.microsoft.com/en-us/ef/core/cli/dotnet

### 3. Startup instruction

1. ```dotnet restore``` or using ide nuget packages restoration tools
2. ```docker-compose -f dev.docker-compose.yml up -d --build```
3. ```dotnet ef database update --project Taledynamic.Core --startup-project Taledynamic.Api```
4. ```dotnet run``` or ide configuration manager