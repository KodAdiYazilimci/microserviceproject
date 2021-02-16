using MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Configuration.Services
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
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<SessionRepository>(x => new SessionRepository(GetAuthorizationConnectionString(configuration)));
            services.AddScoped<UserRepository>(x => new UserRepository(GetAuthorizationConnectionString(configuration)));

            return services;
        }

        /// <summary>
        /// Yetki altyapısı için kullanılacak veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <param name="configuration">Bağlantı cümlesini verecek Configuration nesnesi</param>
        /// <returns></returns>
        private static string GetAuthorizationConnectionString(IConfiguration configuration)
        {
            return
                configuration
                .GetSection("Configuration")
                .GetSection("Authorization")
                .GetSection("DataSource").Value;
        }
    }
}
