
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Caching.InMemory.Mock
{
    /// <summary>
    /// Önbellek sağlayıcısını taklit eden sınıf
    /// </summary>
    public class InMemoryCacheDataProviderFactory
    {
        /// <summary>
        /// Önbellek sağlayıcısı örneği
        /// </summary>
        public static InMemoryCacheDataProvider Instance
        {
            get
            {
                return new InMemoryCacheDataProvider(new MemoryCache(new MemoryCacheOptions()));
            }
        }
    }
}
