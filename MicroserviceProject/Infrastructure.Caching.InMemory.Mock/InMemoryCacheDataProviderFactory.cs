
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching.InMemory.Mock
{
    /// <summary>
    /// Önbellek sağlayıcısını taklit eden sınıf
    /// </summary>
    public class InMemoryCacheDataProviderFactory
    {
        /// <summary>
        /// Önbellek sağlayıcısı
        /// </summary>
        private static InMemoryCacheDataProvider memoryCache = null;

        /// <summary>
        /// Önbellek sağlayıcısı örneği
        /// </summary>
        public static InMemoryCacheDataProvider Instance
        {
            get
            {
                if (memoryCache == null)
                {
                    memoryCache = new InMemoryCacheDataProvider(new MemoryCache(new MemoryCacheOptions()));
                }

                return memoryCache;
            }
        }
    }
}
