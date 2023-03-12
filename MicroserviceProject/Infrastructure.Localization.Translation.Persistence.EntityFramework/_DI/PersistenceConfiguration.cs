using Infrastructure.Localization.Translation.Persistence.Abstract;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Diagnostics;

namespace Infrastructure.Localization.Translation.Persistence.EntityFramework.DI
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
        public static IServiceCollection RegisterEntityFrameworkLocalizationPersistence(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.AddDbContext<TranslationDbContext>(optionBuilder =>
            {
                optionBuilder.UseSqlServer(
                    Convert.ToBoolean(
                        configuration
                        .GetSection("Configuration")
                        .GetSection("Localization")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                        ?
                        Environment.GetEnvironmentVariable(
                            configuration
                            .GetSection("Configuration")
                            .GetSection("Localization")["EnvironmentVariableName"])
                        :
                        configuration
                        .GetSection("Configuration")
                        .GetSection("Localization")["TranslationDbConnnectionString"]);

                optionBuilder.EnableSensitiveDataLogging();
                optionBuilder.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            services.AddScoped<ITranslationRepository, EfTranslationRepository>();

            return services;
        }
    }
}
