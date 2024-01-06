#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Services.Api.ServiceDiscovery/Services.Api.ServiceDiscovery.csproj", "Services.Api.ServiceDiscovery/"]
RUN dotnet restore "Services.Api.ServiceDiscovery/Services.Api.ServiceDiscovery.csproj"
COPY . .
WORKDIR "/src/Services.Api.ServiceDiscovery"
RUN dotnet build "Services.Api.ServiceDiscovery.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Services.Api.ServiceDiscovery.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.Api.ServiceDiscovery.dll"]