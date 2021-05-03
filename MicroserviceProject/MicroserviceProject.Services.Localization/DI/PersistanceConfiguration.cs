using MicroserviceProject.Infrastructure.Localization.Persistence.Configuration;
using MicroserviceProject.Services.Localization.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Localization.DI
{
    /// <summary>
    /// Bölgesel ayarlar veri saklayıcılarının DI sınıfı
    /// </summary>
    public static class PersistanceConfiguration
    {
        /// <summary>
        /// Bölgesel ayarlar veri sağlayıcılarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterLocalizationPersistence(this IServiceCollection services)
        {
            services.AddDbContext<TranslationDbContext>();
            services.AddSingleton<TranslationRepository>();

            return services;
        }
    }
}
