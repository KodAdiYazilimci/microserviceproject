
using Microsoft.Extensions.DependencyInjection;

using Services.Logging.RequestResponse.Persistence;

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
