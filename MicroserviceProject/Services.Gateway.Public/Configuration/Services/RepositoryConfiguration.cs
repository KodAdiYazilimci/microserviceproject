
using MicroserviceProject.Services.Gateway.Public.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Gateway.Public.Configuration.Services
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
            services.AddScoped<UserRepository>();

            return services;
        }
    }
}
