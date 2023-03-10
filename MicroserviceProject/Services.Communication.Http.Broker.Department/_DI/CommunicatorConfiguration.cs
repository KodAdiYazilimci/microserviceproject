using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Department.Abstract;
using Services.Communication.Http.Broker.DI;

namespace Services.Communication.Http.Broker.Department.DI
{
    public static class CommunicatorConfiguration
    {
        public static IServiceCollection RegisterDepartmentCommunicator(this IServiceCollection services)
        {
            services.RegisterCredentialProvider();
            services.RegisterDefaultCommunicator();
            services.RegisterInMemoryCaching();
            services.RegisterRoutingProviders();

            services.RegisterHttpAuthorizationCommunicators();
            services.AddSingleton<IDepartmentCommunicator, DepartmentCommunicator>();

            return services;
        }
    }
}
