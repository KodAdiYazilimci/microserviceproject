
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Localization.Configuration.Persistence;
using Services.Api.Localization.Repositories;

namespace Services.Api.Localization.DI
{
    /// <summary>
    /// Bölgesel ayarlar sağlayıcılarının DI sınıfı
    /// </summary>
    public static class PersistenceConfiguration
    {
        /// <summary>
        /// Bölgesel ayarlar sağlayıcılarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterPersistence(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddDbContext<TranslationDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(
                    configuration
                    .GetSection("Persistence")["DataSource"]);

                optionBuilder.EnableSensitiveDataLogging();
                optionBuilder.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            services.AddScoped<TranslationRepository>();

            return services;
        }
    }
}
