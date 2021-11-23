
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Caching.InMemory.DI
{
    /// <summary>
    /// InMemory Cache işlemleri DI sınıfı
    /// </summary>
    public static class InMemoryCacheConfiguration
    {
        /// <summary>
        /// InMemory Cacheleri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterInMemoryCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<InMemoryCacheDataProvider>();

            return services;
        }
    }
}
