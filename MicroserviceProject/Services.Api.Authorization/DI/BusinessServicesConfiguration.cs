
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Authorization.Business.Services;

namespace Services.Api.Authorization.DI
{
    /// <summary>
    /// İş mantığı servisleri DI sınıfı
    /// </summary>
    public static class BusinessServicesConfiguration
    {
        /// <summary>
        /// İş mantığı servislerini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<SessionService>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
