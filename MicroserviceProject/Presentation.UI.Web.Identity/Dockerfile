#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation.UI.Web.Identity/Presentation.UI.Web.Identity.csproj", "Presentation.UI.Web.Identity/"]
COPY ["Infrastructure.Security.Authentication.BasicToken/Infrastructure.Security.Authentication.BasicToken.csproj", "Infrastructure.Security.Authentication.BasicToken/"]
COPY ["Infrastructure.Security.Authentication/Infrastructure.Security.Authentication.csproj", "Infrastructure.Security.Authentication/"]
COPY ["Infrastructure.Security/Infrastructure.Security.csproj", "Infrastructure.Security/"]
COPY ["Services.Communication.Http.Broker.Authorization/Services.Communication.Http.Broker.Authorization.csproj", "Services.Communication.Http.Broker.Authorization/"]
COPY ["Infrastructure.ServiceDiscovery.Discoverer/Infrastructure.ServiceDiscovery.Discoverer.csproj", "Infrastructure.ServiceDiscovery.Discoverer/"]
COPY ["Infrastructure.Caching.InMemory/Infrastructure.Caching.InMemory.csproj", "Infrastructure.Caching.InMemory/"]
COPY ["Infrastructure.Caching/Infrastructure.Caching.csproj", "Infrastructure.Caching/"]
COPY ["Infrastructure.Communication.Http.Broker/Infrastructure.Communication.Http.Broker.csproj", "Infrastructure.Communication.Http.Broker/"]
COPY ["Infrastructure.Communication.Http.Endpoint/Infrastructure.Communication.Http.Endpoint.csproj", "Infrastructure.Communication.Http.Endpoint/"]
COPY ["Infrastructure.Communication.Http/Infrastructure.Communication.Http.csproj", "Infrastructure.Communication.Http/"]
COPY ["Infrastructure.Validation/Infrastructure.Validation.csproj", "Infrastructure.Validation/"]
COPY ["Infrastructure.ServiceDiscovery/Infrastructure.ServiceDiscovery.csproj", "Infrastructure.ServiceDiscovery/"]
COPY ["Services.Communication.Http.Broker/Services.Communication.Http.Broker.csproj", "Services.Communication.Http.Broker/"]
COPY ["Services.Communication.Http.Endpoint.Authorization/Services.Communication.Http.Endpoint.Authorization.csproj", "Services.Communication.Http.Endpoint.Authorization/"]
COPY ["Services.Communication.Http.Endpoint.Presentation.UI.Web.Identity/Services.Communication.Http.Endpoint.Presentation.UI.Web.Identity.csproj", "Services.Communication.Http.Endpoint.Presentation.UI.Web.Identity/"]
COPY ["Services.Logging.Exception/Services.Logging.Exception.csproj", "Services.Logging.Exception/"]
COPY ["Infrastructure.Logging.Elastic/Infrastructure.Logging.Elastic.csproj", "Infrastructure.Logging.Elastic/"]
COPY ["Infrastructure.Logging/Infrastructure.Logging.csproj", "Infrastructure.Logging/"]
COPY ["Infrastructure.Logging.File/Infrastructure.Logging.File.csproj", "Infrastructure.Logging.File/"]
COPY ["Infrastructure.Logging.RabbitMq/Infrastructure.Logging.RabbitMq.csproj", "Infrastructure.Logging.RabbitMq/"]
COPY ["Infrastructure.Communication.Mq.Rabbit/Infrastructure.Communication.Mq.Rabbit.csproj", "Infrastructure.Communication.Mq.Rabbit/"]
COPY ["Infrastructure.Communication.Mq/Infrastructure.Communication.Mq.csproj", "Infrastructure.Communication.Mq/"]
COPY ["Services.ServiceDiscovery/Services.ServiceDiscovery.csproj", "Services.ServiceDiscovery/"]
COPY ["Infrastructure.ServiceDiscovery.Register/Infrastructure.ServiceDiscovery.Register.csproj", "Infrastructure.ServiceDiscovery.Register/"]
COPY ["Services.Util/Services.Util.csproj", "Services.Util/"]
RUN dotnet restore "./Presentation.UI.Web.Identity/./Presentation.UI.Web.Identity.csproj"
COPY . .
WORKDIR "/src/Presentation.UI.Web.Identity"
RUN dotnet build "./Presentation.UI.Web.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Presentation.UI.Web.Identity.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Presentation.UI.Web.Identity.dll"]