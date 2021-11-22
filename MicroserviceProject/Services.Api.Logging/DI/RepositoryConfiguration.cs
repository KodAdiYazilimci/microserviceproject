
using Infrastructure.Logging.Logger.RequestResponseLogger.Persistence;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Api.Infrastructure.Logging.Configuration.Services.Repositories
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
            services.AddSingleton<RequestResponseLogRepository>();

            return services;
        }
    }
}
