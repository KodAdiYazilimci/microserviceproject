
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Accounting.Configuration.Persistence;
using Services.Api.Business.Departments.Accounting.Repositories.Sql;

namespace Services.Api.Business.Departments.Accounting.DI
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

            services.AddScoped<BankAccountRepository>();
            services.AddScoped<CurrencyRepository>();
            services.AddScoped<SalaryPaymentRepository>();

            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionItemRepository>();

            return services;
        }
    }
}
