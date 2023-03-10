
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Finance.Configuration.Persistence;
using Services.Business.Departments.Finance.Repositories.Sql;

namespace Services.Api.Business.Departments.Buying.DI
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<DecidedCostRepository>();
            services.AddScoped<ProductionRequestRepository>();  
            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionItemRepository>();

            return services;
        }
    }
}
