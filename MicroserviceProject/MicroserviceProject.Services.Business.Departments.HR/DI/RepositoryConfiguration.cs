using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.HR.DI
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
            services.AddScoped<DepartmentRepository>();
            services.AddScoped<PersonRepository>();
            services.AddScoped<TitleRepository>();
            services.AddScoped<WorkerRepository>();
            services.AddScoped<WorkerRelationRepository>();

            return services;
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
