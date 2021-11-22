
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Infrastructure.Authorization.Repositories;

namespace Services.Api.Infrastructure.Authorization.DI
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
            services.AddScoped<ClaimRepository>();
            services.AddScoped<ClaimTypeRepository>();
            services.AddScoped<PolicyRepository>();
            services.AddScoped<PolicyRoleRepository>();
            services.AddScoped<RoleRepository>();
            services.AddScoped<SessionRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<UserRoleRepository>();

            return services;
        }
    }
}
