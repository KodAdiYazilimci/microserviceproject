
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Gateway.Public.Util.Communication;

namespace Services.Api.Gateway.Public.DI
{
    /// <summary>
    /// Araçların DI sınıfı
    /// </summary>
    public static class UtilityConfiguration
    {
        /// <summary>
        /// Araçları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterUtilities(this IServiceCollection services)
        {
            services.AddScoped<ApiBridge>();

            return services;
        }
    }
}
