
using Infrastructure.Caching.Abstraction;

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
        public static IInMemoryCacheDataProvider Instance
        {
            get
            {
                return new InMemoryCacheDataProvider(new MemoryCache(new MemoryCacheOptions()));
            }
        }
    }
}
