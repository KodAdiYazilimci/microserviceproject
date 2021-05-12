using MicroserviceProject.Infrastructure.Localization.Helpers;
using MicroserviceProject.Infrastructure.Localization.Providers;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Infrastructure.Localization.DI
{
    /// <summary>
    /// Bölgesel ayarlar sağlayıcılarının DI sınıfı
    /// </summary>
    public static class ProviderConfiguration
    {
        /// <summary>
        /// Bölgesel ayarlar sağlayıcılarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterLocalizationProviders(this IServiceCollection services)
        {
            services.AddSingleton<TranslationHelper>();
            services.AddSingleton<TranslationProvider>();

            return services;
        }
    }
}
