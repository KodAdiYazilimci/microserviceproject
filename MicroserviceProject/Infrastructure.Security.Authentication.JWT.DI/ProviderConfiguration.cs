using Infrastructure.Security.Authentication.JWT.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.JWT.DI
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
