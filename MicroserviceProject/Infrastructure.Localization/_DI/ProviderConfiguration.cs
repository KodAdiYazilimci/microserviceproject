using Infrastructure.Caching.Redis.DI;
using Infrastructure.Localization.Configuration;
using Infrastructure.Localization.Helpers;
using Infrastructure.Localization.Providers;
using Infrastructure.Localization.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Localization.DI
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

            services.AddScoped<TranslationHelper>();
            services.AddScoped<TranslationProvider>();

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
