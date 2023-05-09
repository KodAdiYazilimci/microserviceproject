using Infrastructure.Caching.Abstraction;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Communication.Http.Models;
using Infrastructure.ServiceDiscovery.Models;
using Infrastructure.ServiceDiscovery.Register.DI;

using Services.Api.ServiceDiscovery.Dto;
using Services.Communication.Http.Endpoint.ServiceDiscovery;

using System.Net;

namespace Services.Api.ServiceDiscovery.DI
{
    public static class ServiceDiscoveryConfiguration
    {
        public static IServiceCollection RegisterServiceDiscoveries(this IServiceCollection services)
        {
            services.RegisterServiceRegisterers();

            return services;
        }

        public static IApplicationBuilder RegisterService(
            this IApplicationBuilder applicationBuilder,
            IConfiguration configuration,
            IDistrubutedCacheProvider distrubutedCacheProvider,
            IInMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            ServiceDto serviceDto = new ServiceDto();
            serviceDto.Protocol = configuration.GetSection("Configuration").GetSection("ServiceDiscovery").GetSection("Registeration")["Protocol"];
            serviceDto.ServiceName = configuration.GetSection("Configuration").GetSection("ServiceDiscovery").GetSection("Registeration")["ServiceName"];
            serviceDto.Port = Convert.ToInt32(configuration.GetSection("Configuration").GetSection("ServiceDiscovery").GetSection("Registeration")["Port"]);
            serviceDto.DnsName =
                Convert.ToBoolean(configuration.GetSection("Configuration").GetSection("ServiceDiscovery").GetSection("Registeration")["OverrideDnsName"])
                ?
                configuration.GetSection("Configuration").GetSection("ServiceDiscovery").GetSection("Registeration")["OverridenDnsName"]
                :
                Dns.GetHostName();
            serviceDto.IpAddresses = Dns.GetHostByName(Dns.GetHostName()).AddressList.Select(x => new IpDto()
            {
                Address = x.ToString(),
                AddressFamily = x.AddressFamily
            }).ToList();

            var discoveredServicesEndpoint = new GetDiscoveredServicesEndpoint();
            var dropServiceEndpoint = new DropServiceEndpoint();

            serviceDto.Endpoints = new List<EndpointDto>()
            {
                new EndpointDto()
                {
                     AuthenticationType = discoveredServicesEndpoint.AuthenticationType,
                     Headers = discoveredServicesEndpoint.Headers,
                     HttpAction = discoveredServicesEndpoint.HttpAction,
                     Name = discoveredServicesEndpoint.Name,
                     Payload = discoveredServicesEndpoint.Payload,
                     Queries = discoveredServicesEndpoint.Queries,
                     StatusCodes = discoveredServicesEndpoint.StatusCodes,
                     Url = discoveredServicesEndpoint.Url
                },
                new EndpointDto()
                {
                    AuthenticationType = dropServiceEndpoint.AuthenticationType,
                    Headers = dropServiceEndpoint.Headers,
                    Queries = dropServiceEndpoint.Queries,
                    HttpAction = dropServiceEndpoint.HttpAction,
                    Name = dropServiceEndpoint.Name,
                    Payload = dropServiceEndpoint.Payload,
                    StatusCodes = dropServiceEndpoint.StatusCodes,
                    Url = dropServiceEndpoint.Url
                }
            };

            distrubutedCacheProvider.Set<ServiceDto>($"Cached_Services_{serviceDto.ServiceName}", serviceDto, DateTime.UtcNow.AddYears(1));
            List<string> discoveredServices = new List<string>();

            if (inMemoryCacheDataProvider.TryGetValue<List<string>>("Cached_Services", out List<string> _discoveredServices))
            {
                discoveredServices = _discoveredServices;
            }

            discoveredServices.Add(serviceDto.ServiceName);

            inMemoryCacheDataProvider.Set<List<string>>("Cached_Services", discoveredServices);

            return applicationBuilder;
        }
    }
}
