using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// Önbellek sağlayıcısını taklit eden sınıf
    /// </summary>
    public class MemoryCacheFactory
    {
        /// <summary>
        /// Önbellek sağlayıcısı
        /// </summary>
        private static IMemoryCache memoryCache = null;

        /// <summary>
        /// Önbellek sağlayıcısı örneği
        /// </summary>
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
