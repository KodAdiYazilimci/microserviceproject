using MicroserviceProject.Infrastructure.Communication.Moderator.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Authentication
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
