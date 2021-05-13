using Infrastructure.Caching.Redis;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class CacheDataProviderFactory
    {
        private static CacheDataProvider cacheDataProvider = null;

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
