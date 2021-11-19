
using Infrastructure.Localization.Configuration;
using Infrastructure.Localization.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Localization.DI
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
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddDbContext<TranslationDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("Localization")["TranslationDbConnnectionString"]);

                optionBuilder.EnableSensitiveDataLogging();
                optionBuilder.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            services.AddScoped<TranslationRepository>();

            return services;
        }
    }
}
