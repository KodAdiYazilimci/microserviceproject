using Infrastructure.Caching.Redis;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// Redis önbellek sağlayıcısını taklit eden sınıf
    /// </summary>
    public class CacheDataProviderFactory
    {
        /// <summary>
        /// Önbellek sağlayıcısı
        /// </summary>
        private static CacheDataProvider cacheDataProvider = null;

        /// <summary>
        /// Önbellek sağlyıcısının örneğini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static CacheDataProvider GetInstance(IConfiguration configuration)
        {
            if (cacheDataProvider == null)
            {
                cacheDataProvider = new CacheDataProvider(configuration);
            }

            return cacheDataProvider;
        }
    }
}
