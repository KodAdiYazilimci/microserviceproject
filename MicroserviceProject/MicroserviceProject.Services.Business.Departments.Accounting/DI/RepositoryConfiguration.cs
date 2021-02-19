﻿
using MicroserviceProject.Services.Business.Departments.Accounting.Repositories.Sql;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.Accounting.DI
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
            services.AddScoped<BankAccountRepository>();
            services.AddScoped<CurrencyRepository>();
            services.AddScoped<SalaryPaymentRepository>();

            return services;
        }
    }
}
