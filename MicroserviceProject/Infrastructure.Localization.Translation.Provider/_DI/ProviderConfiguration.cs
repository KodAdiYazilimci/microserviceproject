using Infrastructure.Caching.Redis.DI;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.DI;
using Infrastructure.Localization.Translation.Provider.Helpers;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Localization.Translation.Provider.DI
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
            services.RegisterRedisCaching();
            services.RegisterEntityFrameworkLocalizationPersistence();

            services.AddScoped<TranslationHelper>();
            services.AddScoped<TranslationProvider>();


            return services;
        }
    }
}
