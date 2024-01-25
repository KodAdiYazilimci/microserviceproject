#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services.Api.Logging/Services.Api.Logging.csproj", "Services.Api.Logging/"]
COPY ["Infrastructure.Caching.Redis/Infrastructure.Caching.Redis.csproj", "Infrastructure.Caching.Redis/"]
COPY ["Infrastructure.Caching/Infrastructure.Caching.csproj", "Infrastructure.Caching/"]
COPY ["Infrastructure.Diagnostics.HealthCheck.Util/Infrastructure.Diagnostics.HealthCheck.Util.csproj", "Infrastructure.Diagnostics.HealthCheck.Util/"]
COPY ["Infrastructure.Communication.Http/Infrastructure.Communication.Http.csproj", "Infrastructure.Communication.Http/"]
COPY ["Infrastructure.Validation/Infrastructure.Validation.csproj", "Infrastructure.Validation/"]
COPY ["Infrastructure.Logging.MongoDb/Infrastructure.Logging.MongoDb.csproj", "Infrastructure.Logging.MongoDb/"]
COPY ["Infrastructure.Logging/Infrastructure.Logging.csproj", "Infrastructure.Logging/"]
COPY ["Infrastructure.Communication.Http.Wrapper/Infrastructure.Communication.Http.Wrapper.csproj", "Infrastructure.Communication.Http.Wrapper/"]
COPY ["Infrastructure.Transaction.Recovery/Infrastructure.Transaction.Recovery.csproj", "Infrastructure.Transaction.Recovery/"]
COPY ["Infrastructure.Transaction.UnitOfWork.Sql/Infrastructure.Transaction.UnitOfWork.Sql.csproj", "Infrastructure.Transaction.UnitOfWork.Sql/"]
COPY ["Infrastructure.Transaction.UnitOfWork/Infrastructure.Transaction.UnitOfWork.csproj", "Infrastructure.Transaction.UnitOfWork/"]
COPY ["Infrastructure.Util.DI/Infrastructure.Util.DI.csproj", "Infrastructure.Util.DI/"]
COPY ["Services.Communication.Http.Endpoint.Logging/Services.Communication.Http.Endpoint.Logging.csproj", "Services.Communication.Http.Endpoint.Logging/"]
COPY ["Infrastructure.Communication.Http.Endpoint/Infrastructure.Communication.Http.Endpoint.csproj", "Infrastructure.Communication.Http.Endpoint/"]
COPY ["Services.Diagnostics.HealthCheck/Services.Diagnostics.HealthCheck.csproj", "Services.Diagnostics.HealthCheck/"]
COPY ["Infrastructure.Diagnostics.HealthCheck.Actions/Infrastructure.Diagnostics.HealthCheck.Actions.csproj", "Infrastructure.Diagnostics.HealthCheck.Actions/"]
COPY ["Services.Logging.RequestResponse/Services.Logging.RequestResponse.csproj", "Services.Logging.RequestResponse/"]
COPY ["Infrastructure.Logging.Elastic/Infrastructure.Logging.Elastic.csproj", "Infrastructure.Logging.Elastic/"]
COPY ["Infrastructure.Logging.File/Infrastructure.Logging.File.csproj", "Infrastructure.Logging.File/"]
COPY ["Infrastructure.Logging.RabbitMq/Infrastructure.Logging.RabbitMq.csproj", "Infrastructure.Logging.RabbitMq/"]
COPY ["Infrastructure.Communication.Mq.Rabbit/Infrastructure.Communication.Mq.Rabbit.csproj", "Infrastructure.Communication.Mq.Rabbit/"]
COPY ["Infrastructure.Communication.Mq/Infrastructure.Communication.Mq.csproj", "Infrastructure.Communication.Mq/"]
COPY ["Services.Security.BasicToken/Services.Security.BasicToken.csproj", "Services.Security.BasicToken/"]
COPY ["Infrastructure.Security.Authentication.BasicToken/Infrastructure.Security.Authentication.BasicToken.csproj", "Infrastructure.Security.Authentication.BasicToken/"]
COPY ["Infrastructure.Security.Authentication/Infrastructure.Security.Authentication.csproj", "Infrastructure.Security.Authentication/"]
COPY ["Infrastructure.Security/Infrastructure.Security.csproj", "Infrastructure.Security/"]
COPY ["Services.Communication.Http.Broker.Authorization/Services.Communication.Http.Broker.Authorization.csproj", "Services.Communication.Http.Broker.Authorization/"]
COPY ["Infrastructure.ServiceDiscovery.Discoverer/Infrastructure.ServiceDiscovery.Discoverer.csproj", "Infrastructure.ServiceDiscovery.Discoverer/"]
COPY ["Infrastructure.Caching.InMemory/Infrastructure.Caching.InMemory.csproj", "Infrastructure.Caching.InMemory/"]
COPY ["Infrastructure.Communication.Http.Broker/Infrastructure.Communication.Http.Broker.csproj", "Infrastructure.Communication.Http.Broker/"]
COPY ["Infrastructure.ServiceDiscovery/Infrastructure.ServiceDiscovery.csproj", "Infrastructure.ServiceDiscovery/"]
COPY ["Services.Communication.Http.Broker/Services.Communication.Http.Broker.csproj", "Services.Communication.Http.Broker/"]
COPY ["Services.Communication.Http.Endpoint.Authorization/Services.Communication.Http.Endpoint.Authorization.csproj", "Services.Communication.Http.Endpoint.Authorization/"]
COPY ["Services.ServiceDiscovery/Services.ServiceDiscovery.csproj", "Services.ServiceDiscovery/"]
COPY ["Infrastructure.ServiceDiscovery.Register/Infrastructure.ServiceDiscovery.Register.csproj", "Infrastructure.ServiceDiscovery.Register/"]
COPY ["Services.Util/Services.Util.csproj", "Services.Util/"]
COPY ["Services.Logging.Exception/Services.Logging.Exception.csproj", "Services.Logging.Exception/"]
RUN dotnet restore "./Services.Api.Logging/./Services.Api.Logging.csproj"
COPY . .
WORKDIR "/src/Services.Api.Logging"
RUN dotnet build "./Services.Api.Logging.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Services.Api.Logging.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.Api.Logging.dll"]