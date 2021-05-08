using MicroserviceProject.Infrastructure.Caching.Redis;
using MicroserviceProject.Infrastructure.Localization.Helpers;
using MicroserviceProject.Infrastructure.Localization.Models;
using MicroserviceProject.Services.Localization.Entities;
using MicroserviceProject.Services.Localization.Repositories;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Localization.Providers
{
    /// <summary>
    /// Dil çeviri sağlayıcısı sınıf
    /// </summary>
    public class TranslationProvider : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çeviri işlemlerinin yapılandırma bilgilerini tutan configuration nesnesi
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Önbellekteki çevirileri yönetecek sağlayıcı
        /// </summary>
        private readonly CacheDataProvider cacheDataProvider;

        /// <summary>
        /// Dil çeviri kayıtlarını yöneten repository sınıfı
        /// </summary>
        private TranslationRepository translationRepository;

        /// <summary>
        /// Dil çeviri yardımcısı
        /// </summary>
        private readonly TranslationHelper translationHelper;

        /// <summary>
        /// Dil çeviri sağlayıcısı sınıf
        /// </summary>
        /// <param name="configuration">Çeviri işlemlerinin yapılandırma bilgilerini tutan configuration nesnesi</param>
        /// <param name="cacheDataProvider">Önbellekteki çevirileri yönetecek sağlayıcı</param>
        /// <param name="translationRepository">Dil çeviri kayıtlarını yöneten repository sınıfı</param>
        /// <param name="translationHelper">Dil çeviri yardımcısı</param>
        public TranslationProvider(
            IConfiguration configuration,
            CacheDataProvider cacheDataProvider,
            TranslationRepository translationRepository,
            TranslationHelper translationHelper)
        {
            this.configuration = configuration;
            this.cacheDataProvider = cacheDataProvider;
            this.translationRepository = translationRepository;
            this.translationHelper = translationHelper;
        }

        /// <summary>
        /// Çeviri işlemini yapar
        /// </summary>
        /// <param name="key">Çevirinin anahtarı</param>
        /// <param name="regionCode">Çevirinin yapılacağı bölge kodu</param>
        /// <param name="parameters">Çeviride kullanılacak parametreler</param>
        /// <returns></returns>
        public string Translate(string key, string regionCode, List<KeyValuePair<string, string>> parameters)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                regionCode = configuration.GetSection("Configuration").GetSection("Localization")["DefaultRegion"];
            }

            string language = CultureInfo.GetCultureInfo(regionCode).TwoLetterISOLanguageName;

            string cacheKey = configuration.GetSection("Configuration").GetSection("Localization")["CacheKey"];

            if (cacheDataProvider.TryGetValue(cacheKey, out List<TranslationModel> translations))
            {
                if (!translations.Any())
                {
                    List<TranslationEntity> translationEntities = translationRepository.GetTranslations();

                    if (translationEntities.Any())
                    {
                        translations = translationEntities.Select(x => new TranslationModel()
                        {
                            Key = x.Key,
                            LanguageCode = x.LanguageCode,
                            Text = x.Text
                        }).ToList();

                        cacheDataProvider.Set(cacheKey, translations);
                    }
                }
            }

            return translationHelper.Translate(translations, key, language, parameters);
        }

        /// <summary>
        /// Asenkron olarak çeviri işlemini yapar
        /// </summary>
        /// <param name="key">Çevirinin anahtarı</param>
        /// <param name="regionCode">Çevirinin yapılacağı bölge kodu</param>
        /// <param name="parameters">Çeviride kullanılacak parametreler</param>
        /// <returns></returns>
        public async Task<string> TranslateAsync(string key, string regionCode, List<KeyValuePair<string, string>> parameters = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                regionCode = configuration.GetSection("Configuration").GetSection("Localization")["DefaultRegion"];
            }

            string language = CultureInfo.GetCultureInfo(regionCode).TwoLetterISOLanguageName;

            string cacheKey = configuration.GetSection("Configuration").GetSection("Localization")["CacheKey"];

            if (cacheDataProvider.TryGetValue(cacheKey, out List<TranslationModel> translations))
            {
                if (!translations.Any())
                {
                    List<TranslationEntity> translationEntities = await translationRepository.GetTranslationsAsync(cancellationToken);

                    if (translationEntities.Any())
                    {
                        translations = translationEntities.Select(x => new TranslationModel()
                        {
                            Key = x.Key,
                            LanguageCode = x.LanguageCode,
                            Text = x.Text
                        }).ToList();

                        cacheDataProvider.Set(cacheKey, translations);
                    }
                }
            }

            return translationHelper.Translate(translations, key, language, parameters);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    translationRepository?.Dispose();
                    translationRepository = null;
                }

                disposed = true;
            }
        }
    }
}
