using Infrastructure.Persistence.Logging.Sql.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Logging.Configuration.Services.Repositories
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
        /// <returns></returns>
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddSingleton<RequestResponseRepository>();

            return services;
        }
    }
}
