using MicroserviceProject.Infrastructure.Security.Authentication.JWT.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Authentication.JWT.DI
{
    public static class ProviderConfiguration
    {
        public static IServiceCollection RegisterJWTProviders(this IServiceCollection services)
        {
            services.AddSingleton<IssuerProvider>();
            services.AddSingleton<SecretProvider>();
            services.AddSingleton<TokenProvider>();

            return services;
        }
    }
}
