
using Infrastructure.Security.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.BasicToken.DI
{
    /// <summary>
    /// Servis kimliği sağlayıcısı DI sınıfı
    /// </summary>
    public static class CredentialProviderConfiguration
    {
        /// <summary>
        /// Servis kimliği sağlayıcısını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="configuration">Configuration nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterCredentialProvider(this IServiceCollection services)
        {
            services.AddSingleton<CredentialProvider>();

            return services;
        }
    }
}
