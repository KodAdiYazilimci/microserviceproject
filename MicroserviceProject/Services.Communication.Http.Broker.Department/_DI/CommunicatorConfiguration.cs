using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers._DI;
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
            services.RegisterDefaultCommunicator();

            services.RegisterCredentialProvider();
            services.RegisterHttpRouteRepositories();
            services.RegisterRoutingProviders();
            services.RegisterInMemoryCaching();

            services.RegisterHttpAuthorizationCommunicators();
            services.AddSingleton<IDepartmentCommunicator, DepartmentCommunicator>();

            return services;
        }
    }
}
