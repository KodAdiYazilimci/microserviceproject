using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using MicroserviceProject.Services.Business.Departments.HR.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.HR.Configuration.Services
{
    /// <summary>
    /// Repository DI sınıfı
    /// </summary>
    public static class RepositoryConfiguration
    {
        /// <summary>
        /// Repositoryleri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <param name="configuration">Configuration nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x =>
                new ServiceRouteRepository(GetServiceRouteRepositoryConnectionString(configuration)));

            services.AddScoped<DepartmentRepository>();

            return services;
        }

        /// <summary>
        /// Request-response loglarının yazılacağı veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini getirecek configuration</param>
        /// <returns></returns>
        private static string GetRequestResponseRepositoryConnectionString(IConfiguration configuration)
        {
            string connectionString =
                configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("RabbitConfiguration")
                .GetSection("RequestResponseLogging")
                .GetSection("DataSource").Value;

            return connectionString;
        }

        /// <summary>
        /// Servis rotaları repository veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini getirecek configuration</param>
        /// <returns></returns>
        private static string GetServiceRouteRepositoryConnectionString(IConfiguration configuration)
        {
            string connectionString =
                configuration
                .GetSection("Configuration")
                .GetSection("Routing")
                .GetSection("DataSource").Value;

            return connectionString;
        }
    }
}
