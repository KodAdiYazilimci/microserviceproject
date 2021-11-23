using Infrastructure.Caching.Redis;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;
using Infrastructure.Localization.Translation.Provider;
using Infrastructure.Localization.Translation.Provider.Helpers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// Dil çeviri sağlayıcısını taklit eden sınıf
    /// </summary>
    public class TranslationProviderFactory
    {
        /// <summary>
        /// Dil çeviri yardımcısı
        /// </summary>
        private static TranslationProvider translationProvider;

        /// <summary>
        /// Dil çeviri sağlayıcısının nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <param name="cacheDataProvider">Önbellek sağlayıcısının nesnesi</param>
        /// <param name="translationRepository">Dil çeviri repository sınıfı nesnesi</param>
        /// <param name="translationHelper">Dil çeviri yardımcısı nesnesi</param>
        /// <returns></returns>
        public static TranslationProvider GetTranslationProvider(
            IConfiguration configuration,
            RedisCacheDataProvider cacheDataProvider,
            TranslationRepository translationRepository,
            TranslationHelper translationHelper)
        {
            if (translationProvider == null)
            {
                translationProvider = new TranslationProvider(configuration, cacheDataProvider, translationRepository, translationHelper);
            }

            return translationProvider;
        }
    }
}
