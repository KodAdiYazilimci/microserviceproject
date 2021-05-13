using Infrastructure.Security.Authentication.JWT.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.JWT.DI
{
    /// <summary>
    /// JWT sağlayıcıları DI sınıfı
    /// </summary>
    public static class ProviderConfiguration
    {
        /// <summary>
        /// JWT sağlayıcılarını enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterJWTProviders(this IServiceCollection services)
        {
            services.AddSingleton<IssuerProvider>();
            services.AddSingleton<SecretProvider>();
            services.AddSingleton<TokenProvider>();

            return services;
        }
    }
}
