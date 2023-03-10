
using Infrastructure.Security.Authentication.Abstract;
using Infrastructure.Security.Authentication.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Security.Authentication.DI
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
            services.AddSingleton<ICredentialProvider, AppConfigCredentialProvider>();

            return services;
        }
    }
}
