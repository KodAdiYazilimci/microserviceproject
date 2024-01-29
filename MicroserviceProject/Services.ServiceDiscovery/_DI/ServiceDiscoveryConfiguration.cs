using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.ServiceDiscovery.Models;
using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using System.Net;

namespace Services.ServiceDiscovery.DI
{
    public static class ServiceDiscoveryConfiguration
    {
        public static IApplicationBuilder RegisterService(this IApplicationBuilder applicationBuilder, List<IEndpoint> endpoints)
        {
            while (true)
            {
                try
                {
                    IServiceRegisterer serviceRegisterer = applicationBuilder.ApplicationServices.GetRequiredService<IServiceRegisterer>();
                    IRegisterationConfiguration registerationConfiguration = applicationBuilder.ApplicationServices.GetRequiredService<IRegisterationConfiguration>();

                    Task registerServiceTask = serviceRegisterer.RegisterServiceAsync(new RegisteredServiceModel()
                    {
                        ServiceName = registerationConfiguration.ServiceName,
                        Port = registerationConfiguration.Port,
                        Protocol = registerationConfiguration.Protocol,
                        Endpoints = endpoints,
                        IpAddresses = Dns.GetHostByName(Dns.GetHostName()).AddressList.Select(x => new IpModel()
                        {
                            Address = x.ToString(),
                            AddressFamily = x.AddressFamily
                        }).ToList(),
                        DnsName = registerationConfiguration.OverrideDnsName ? registerationConfiguration.OverridenDnsName : Dns.GetHostName()
                    }, new CancellationTokenSource());

                    registerServiceTask.Wait();
                    break;
                }
                catch (Exception ex)
                {

                }
            }
            return applicationBuilder;
        }
    }
}
