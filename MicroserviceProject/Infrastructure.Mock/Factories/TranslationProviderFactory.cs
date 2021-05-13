using Infrastructure.Caching.Redis;
using Infrastructure.Localization.Helpers;
using Infrastructure.Localization.Providers;
using Infrastructure.Localization.Repositories;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Factories
{
    public class TranslationProviderFactory
    {
        private static TranslationProvider translationProvider;

        public static TranslationProvider GetTranslationProvider(
            IConfiguration configuration,
            CacheDataProvider cacheDataProvider,
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
