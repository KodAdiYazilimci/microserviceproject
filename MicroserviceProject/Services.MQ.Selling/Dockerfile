#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services.MQ.Selling/Services.MQ.Selling.csproj", "Services.MQ.Selling/"]
COPY ["Infrastructure.Localization.Translation.Provider/Infrastructure.Localization.Translation.Provider.csproj", "Infrastructure.Localization.Translation.Provider/"]
COPY ["Infrastructure.Caching.Redis/Infrastructure.Caching.Redis.csproj", "Infrastructure.Caching.Redis/"]
COPY ["Infrastructure.Caching/Infrastructure.Caching.csproj", "Infrastructure.Caching/"]
COPY ["Infrastructure.Localization.Translation.Persistence.EntityFramework/Infrastructure.Localization.Translation.Persistence.EntityFramework.csproj", "Infrastructure.Localization.Translation.Persistence.EntityFramework/"]
COPY ["Infrastructure.Localization.Translation.Persistence/Infrastructure.Localization.Translation.Persistence.csproj", "Infrastructure.Localization.Translation.Persistence/"]
COPY ["Infrastructure.Localization.Translation/Infrastructure.Localization.Translation.csproj", "Infrastructure.Localization.Translation/"]
COPY ["Infrastructure.Security.Authentication.BasicToken/Infrastructure.Security.Authentication.BasicToken.csproj", "Infrastructure.Security.Authentication.BasicToken/"]
COPY ["Infrastructure.Security.Authentication/Infrastructure.Security.Authentication.csproj", "Infrastructure.Security.Authentication/"]
COPY ["Infrastructure.Security/Infrastructure.Security.csproj", "Infrastructure.Security/"]
COPY ["Infrastructure.Util.DI/Infrastructure.Util.DI.csproj", "Infrastructure.Util.DI/"]
COPY ["Services.Communication.Http.Broker.Department.Selling/Services.Communication.Http.Broker.Department.Selling.csproj", "Services.Communication.Http.Broker.Department.Selling/"]
COPY ["Infrastructure.Transaction.Recovery/Infrastructure.Transaction.Recovery.csproj", "Infrastructure.Transaction.Recovery/"]
COPY ["Services.Communication.Http.Broker.Department/Services.Communication.Http.Broker.Department.csproj", "Services.Communication.Http.Broker.Department/"]
COPY ["Services.Communication.Http.Broker.Authorization/Services.Communication.Http.Broker.Authorization.csproj", "Services.Communication.Http.Broker.Authorization/"]
COPY ["Infrastructure.ServiceDiscovery.Discoverer/Infrastructure.ServiceDiscovery.Discoverer.csproj", "Infrastructure.ServiceDiscovery.Discoverer/"]
COPY ["Infrastructure.Caching.InMemory/Infrastructure.Caching.InMemory.csproj", "Infrastructure.Caching.InMemory/"]
COPY ["Infrastructure.Communication.Http.Broker/Infrastructure.Communication.Http.Broker.csproj", "Infrastructure.Communication.Http.Broker/"]
COPY ["Infrastructure.Communication.Http.Endpoint/Infrastructure.Communication.Http.Endpoint.csproj", "Infrastructure.Communication.Http.Endpoint/"]
COPY ["Infrastructure.Communication.Http/Infrastructure.Communication.Http.csproj", "Infrastructure.Communication.Http/"]
COPY ["Infrastructure.Validation/Infrastructure.Validation.csproj", "Infrastructure.Validation/"]
COPY ["Infrastructure.ServiceDiscovery/Infrastructure.ServiceDiscovery.csproj", "Infrastructure.ServiceDiscovery/"]
COPY ["Services.Communication.Http.Broker/Services.Communication.Http.Broker.csproj", "Services.Communication.Http.Broker/"]
COPY ["Services.Communication.Http.Endpoint.Authorization/Services.Communication.Http.Endpoint.Authorization.csproj", "Services.Communication.Http.Endpoint.Authorization/"]
COPY ["Services.Communication.Http.Endpoint.Department.Selling/Services.Communication.Http.Endpoint.Department.Selling.csproj", "Services.Communication.Http.Endpoint.Department.Selling/"]
COPY ["Services.Logging.Exception/Services.Logging.Exception.csproj", "Services.Logging.Exception/"]
COPY ["Infrastructure.Logging.Elastic/Infrastructure.Logging.Elastic.csproj", "Infrastructure.Logging.Elastic/"]
COPY ["Infrastructure.Logging/Infrastructure.Logging.csproj", "Infrastructure.Logging/"]
COPY ["Infrastructure.Logging.File/Infrastructure.Logging.File.csproj", "Infrastructure.Logging.File/"]
COPY ["Infrastructure.Logging.RabbitMq/Infrastructure.Logging.RabbitMq.csproj", "Infrastructure.Logging.RabbitMq/"]
COPY ["Infrastructure.Communication.Mq.Rabbit/Infrastructure.Communication.Mq.Rabbit.csproj", "Infrastructure.Communication.Mq.Rabbit/"]
COPY ["Infrastructure.Communication.Mq/Infrastructure.Communication.Mq.csproj", "Infrastructure.Communication.Mq/"]
COPY ["Services.Util/Services.Util.csproj", "Services.Util/"]
RUN dotnet restore "./Services.MQ.Selling/./Services.MQ.Selling.csproj"
COPY . .
WORKDIR "/src/Services.MQ.Selling"
RUN dotnet build "./Services.MQ.Selling.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Services.MQ.Selling.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Services.MQ.Selling.dll"]