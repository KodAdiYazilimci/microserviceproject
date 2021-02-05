using Infrastructure.Persistence.Logging.Sql.Repositories;
using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Services.Repositories
{
    public static class RequestResponseRepositoryConfiguration
    {
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(x => new RequestResponseRepository(GetRequestResponseRepositoryConnectionString(configuration)));

            services.AddSingleton(x =>
                new ServiceRouteRepository(GetServiceRouteRepositoryConnectionString(configuration)));

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
