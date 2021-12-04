using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Wrapper;
using Infrastructure.Localization.Translation;
using Infrastructure.Localization.Translation.Models;

using Microsoft.Extensions.Configuration;

using Services.Api.Localization.Entities;
using Services.Api.Localization.Repositories;

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Localization.Services
{
    public class TranslationService : BaseService
    {
        private readonly IConfiguration _configuration;
        private readonly RedisCacheDataProvider _redisCacheDataProvider;
        private readonly TranslationHelper _translationHelper;
        private readonly TranslationRepository _translationRepository;

        public TranslationService(
            IConfiguration configuration,
            RedisCacheDataProvider redisCacheDataProvider,
            TranslationHelper translationHelper,
            TranslationRepository translationRepository)
        {
            _configuration = configuration;
            _redisCacheDataProvider = redisCacheDataProvider;
            _translationHelper = translationHelper;
            _translationRepository = translationRepository;
        }

        /// <summary>
        /// İçerisinde çalışılan servisin adı
        /// </summary>
        public override string ServiceName => "Services.Api.Localization.Services.TranslationService";

        /// <summary>
        /// Servisin ait olduğu api servisinin adı
        /// </summary>
        public override string ApiServiceName => "Services.Api.Localization";

        public async Task<List<TranslationModel>> GetTranslationsAsync(CancellationTokenSource cancellationToken)
        {
            return await GetTranslations(cancellationToken);
        }

        private async Task<List<TranslationModel>> GetTranslations(CancellationTokenSource cancellationTokenSource)
        {
            string cacheKey = _configuration.GetSection("Configuration").GetSection("Localization")["CacheKey"];

            List<TranslationModel> translationModels = null;

            if (_redisCacheDataProvider.TryGetValue(cacheKey, out List<TranslationModel> cachedTranslations))
            {
                translationModels = cachedTranslations;
            }

            if (translationModels == null || !translationModels.Any())
            {
                List<TranslationEntity> translationEntities = await _translationRepository.GetTranslationsAsync(cancellationTokenSource);

                if (translationEntities.Any())
                {
                    translationModels = translationEntities.Select(x => new TranslationModel()
                    {
                        Key = x.Key,
                        Region = x.Region,
                        Text = x.Text
                    }).ToList();

                    _redisCacheDataProvider.Set(cacheKey, translationModels);
                }
            }

            return translationModels;
        }

        public async Task<TranslationModel> TranslateAsync(string key, string regionCode, List<KeyValuePair<string, string>> parameters, CancellationTokenSource cancellationTokenSource)
        {
            return new TranslationModel()
            {
                Key = key,
                Region = regionCode,
                Text = await Translate(key, regionCode, parameters, cancellationTokenSource)
            };
        }

        /// <summary>
        /// Asenkron olarak çeviri işlemini yapar
        /// </summary>
        /// <param name="key">Çevirinin anahtarı</param>
        /// <param name="regionCode">Çevirinin yapılacağı bölge kodu</param>
        /// <param name="parameters">Çeviride kullanılacak parametreler</param>
        /// <returns></returns>
        private async Task<string> Translate(string key, string regionCode, List<KeyValuePair<string, string>> parameters = null, CancellationTokenSource cancellationToken = null)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                regionCode = _configuration.GetSection("Configuration").GetSection("Localization")["DefaultRegion"];
            }

            List<TranslationModel> translations = await GetTranslationsAsync(cancellationToken);

            string language = CultureInfo.GetCultureInfo(regionCode).TwoLetterISOLanguageName;

            return _translationHelper.Translate(translations, key, language, parameters);
        }
    }
}
