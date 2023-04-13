using Infrastructure.Communication.Http.Constants;
using Infrastructure.ServiceDiscovery.Constants;
using Infrastructure.ServiceDiscovery.Models;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Authorization.DI
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

            Task registerServiceTask = serviceRegisterer.RegisterServiceAsync(new ServiceModel()
            {
                ServiceName = "Services.Api.Authorization",
                Port = 15455,
                Protocol = "http",
                Endpoints = new List<EndpointModel>()
                {
                    new EndpointModel()
                    {
                        Name = nameof(Controllers.AuthController.GetToken),
                        EndpointAuthentication = EndpointAuthentications.Anonymouse,
                        HttpAction = HttpAction.POST,
                        Url = $"/Auth/GetToken",
                        StatusCodes = new List<HttpStatusCode>() { HttpStatusCode.OK, HttpStatusCode.BadRequest, HttpStatusCode.Unauthorized }
                    }
                },
                IpAddresses = Dns.GetHostByName(Dns.GetHostName()).AddressList.Select(x => new IpModel()
                {
                    Address = x.ToString(),
                    AddressFamily = x.AddressFamily
                }).ToList(),
                DnsName = Dns.GetHostName()
            }, new CancellationTokenSource());

            registerServiceTask.Wait();

            return applicationBuilder;
        }
    }
}
