
using MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.AA.DI
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
            services.AddScoped<InventoryRepository>();
            services.AddScoped<InventoryDefaultsRepository>();
            services.AddScoped<WorkerInventoryRepository>();
            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionItemRepository>();

            return services;
        }
    }
}
