using Microsoft.Extensions.Caching.Memory;

namespace MicroserviceProject.Test.Services.Factories
{
    public class MemoryCacheFactory
    {
        private static IMemoryCache memoryCache = null;

        public static IMemoryCache Instance
        {
            get
            {
                if (memoryCache == null)
                {
                    memoryCache = new MemoryCache(new MemoryCacheOptions());
                }

                return memoryCache;
            }
        }
    }
}
