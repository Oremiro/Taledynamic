FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 5000
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Development"

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Taledynamic.Api.csproj", "Taledynamic.Api.csproj"]
RUN dotnet restore "Taledynamic.Api.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "Taledynamic.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Taledynamic.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Taledynamic.Api.dll"]
