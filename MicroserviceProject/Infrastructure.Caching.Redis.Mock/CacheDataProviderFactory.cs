
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Caching.Redis.Mock
{
    /// <summary>
    /// Redis önbellek sağlayıcısını taklit eden sınıf
    /// </summary>
    public class CacheDataProviderFactory
    {
        /// <summary>
        /// Önbellek sağlyıcısının örneğini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static RedisCacheDataProvider GetInstance(IConfiguration configuration)
        {
            return new RedisCacheDataProvider(configuration);
        }
    }
}
