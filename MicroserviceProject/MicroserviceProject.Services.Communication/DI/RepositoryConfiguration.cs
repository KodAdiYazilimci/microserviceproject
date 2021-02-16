using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Communication.DI
{
    /// <summary>
    /// Servis rotaları repository DI sınıfı
    /// </summary>
    public static class RepositoryConfiguration
    {
        /// <summary>
        /// Servis rotalarını enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRouteRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ServiceRouteRepository>();

            return services;
        }
    }
}
