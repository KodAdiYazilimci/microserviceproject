using Infrastructure.Diagnostics.HealthCheck.Actions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using System.Collections.Generic;

namespace Services.Diagnostics.HealthCheck.DI
{
    /// <summary>
    /// Sağlık DI sınıfı
    /// </summary>
    public static class HealthCheckConfiguration
    {
        /// <summary>
        /// Veritabanı bağlantılarının sağlığını denetleyecek mekanizmayı enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="connectionStrings">Denetlenecek veritabanı bağlantıları</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSqlHealthChecking(this IServiceCollection services, List<string> connectionStrings)
        {
            services
                .AddHealthChecks()
                .AddTypeActivatedCheck<SqlCheck>(
                    name: nameof(SqlCheck),
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "sql" },
                    args: new object[] { connectionStrings });

            return services;
        }

        /// <summary>
        /// Http bağlantılarının sağlığını denetleyecek mekanizmayı enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="connectionStrings">Denetlenecek http bağlantıları</param>
        /// <returns></returns>
        public static IServiceCollection RegisterHttpHealthChecking(this IServiceCollection services, List<string> urls)
        {
            services
                .AddHealthChecks()
                .AddTypeActivatedCheck<HttpCheck>(
                    name: nameof(HttpCheck),
                    failureStatus: HealthStatus.Unhealthy,
                    tags: new string[] { "http" },
                    args: new object[] { urls });

            return services;
        }
    }
}
