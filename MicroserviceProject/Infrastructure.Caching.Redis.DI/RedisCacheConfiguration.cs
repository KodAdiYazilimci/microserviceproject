
using Infrastructure.Caching.Abstraction;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Caching.Redis.DI
{
    /// <summary>
    /// Redis Cache işlemleri DI sınıfı
    /// </summary>
    public static class RedisCacheConfiguration
    {
        /// <summary>
        /// Redis Cacheleri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterRedisCaching(this IServiceCollection services)
        {
            services.AddSingleton<RedisCacheDataProvider>();

            return services;
        }
    }
}
