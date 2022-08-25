
using Infrastructure.Transaction.UnitOfWork.Sql;

using Microsoft.Extensions.DependencyInjection;

using Services.Api.Logging.Configuration.Persistence;
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
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<RequestResponseLogRepository>();

            return services;
        }
    }
}
