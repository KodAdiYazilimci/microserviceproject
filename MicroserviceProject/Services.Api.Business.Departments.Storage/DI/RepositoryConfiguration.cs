
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Storage.Repositories.EntityFramework;

namespace Services.Api.Business.Departments.Storage.DI
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
        public static IServiceCollection RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<StockRepository>();
            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionItemRepository>();

            return services;
        }
    }
}
