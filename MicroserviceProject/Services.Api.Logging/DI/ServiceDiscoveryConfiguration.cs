using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.ServiceDiscovery.Models;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.DI;
using Infrastructure.ServiceDiscovery.Register.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Endpoint.Authorization;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Logging.DI
{
    public static class ServiceDiscoveryConfiguration
    {
        public static IServiceCollection RegisterServiceDiscoveries(this IServiceCollection services)
        {
            services.RegisterServiceRegisterers();

            return services;
        }

        public static IApplicationBuilder RegisterService(this IApplicationBuilder applicationBuilder)
        {
            IServiceRegisterer serviceRegisterer = applicationBuilder.ApplicationServices.GetRequiredService<IServiceRegisterer>();
            IRegisterationConfiguration registerationConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IRegisterationConfiguration>();

            Task registerServiceTask = serviceRegisterer.RegisterServiceAsync(new RegisteredServiceModel()
            {
                ServiceName = "Services.Api.Logging",
                Port = registerationConfiguration.Port,
                Protocol = "http",
                Endpoints = new List<IEndpoint>()
                {
                    new GetTokenEndpoint(),
                    new GetUserEndpoint()
                },
                IpAddresses = Dns.GetHostByName(Dns.GetHostName()).AddressList.Select(x => new IpModel()
                {
                    Address = x.ToString(),
                    AddressFamily = x.AddressFamily
                }).ToList(),
                DnsName = registerationConfiguration.OverrideDnsName ? registerationConfiguration.OverridenDnsName : Dns.GetHostName()
            }, new CancellationTokenSource());

            registerServiceTask.Wait();

            return applicationBuilder;
        }
    }
}
