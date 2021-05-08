using MicroserviceProject.Infrastructure.Caching.Redis;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Test.Services.Factories
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
