using Infrastructure.Localization.Translation.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Localization.Translation
{
    /// <summary>
    /// Çeviri yardımcısı sınıf
    /// </summary>
    public class TranslationHelper
    {
        /// <summary>
        /// Çeviri yapar
        /// </summary>
        /// <param name="translations">Çevirilerin listesi</param>
        /// <param name="key">Çevirinin anahtarı</param>
        /// <param name="language">Çevirisi yapılacak hedef dil</param>
        /// <param name="parameters">Çeviride kullanılacak parametreler</param>
        /// <returns></returns>
        public string Translate(List<TranslationModel> translations, string key, string language, List<KeyValuePair<string, string>> parameters)
        {
            if (translations != null
                &&
                translations.Any(x => x.Key == key && x.Region == language))
            {
                string translation =
                    translations
                    .FirstOrDefault(x =>
                                    x.Key == key
                                    &&
                                    x.Region == language).Text;

                if (parameters != null && parameters.Any())
                {
                    foreach (var parameter in parameters)
                    {
                        string dbParameter = "%" + parameter.Key + "%";

                        translation = translation.Replace(dbParameter, parameter.Value);
                    }
                }

                return translation;
            }
            else
            {
                throw new Exception("Çeviri değeri bulunamadı!");
            }
        }
    }
}
