
using Infrastructure.Routing.Persistence.Repositories.Sql;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Routing.Persistence.DI
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
        public static IServiceCollection RegisterHttpRouteRepositories(this IServiceCollection services)
        {
            services.AddSingleton<ServiceRouteRepository>();

            return services;
        }
    }
}
