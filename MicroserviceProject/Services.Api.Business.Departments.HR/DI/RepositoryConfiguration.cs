
using Services.Api.Business.Departments.HR.Repositories.Sql;

using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Transaction.UnitOfWork.Sql;
using Services.Api.Business.Departments.HR.Configuration.Persistence;

namespace Services.Api.Business.Departments.HR.DI
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
            services.AddScoped<ISqlUnitOfWork, UnitOfWork>();

            services.AddScoped<DepartmentRepository>();
            services.AddScoped<PersonRepository>();
            services.AddScoped<TitleRepository>();
            services.AddScoped<WorkerRepository>();
            services.AddScoped<WorkerRelationRepository>();

            services.AddScoped<TransactionRepository>();
            services.AddScoped<TransactionItemRepository>();

            return services;
        }
    }
}
