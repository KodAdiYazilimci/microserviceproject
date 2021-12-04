using System.Collections.Generic;

namespace Infrastructure.Localization.Translation.Models
{
    /// <summary>
    /// Dil çevirilerinin modeli
    /// </summary>
    public class TranslationModel
    {
        /// <summary>
        /// Çevirinin anahtar değeri
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Çevirinin metni
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Çevirinin bölge kodu
        /// </summary>
        public string Region { get; set; }
        public List<KeyValuePair<string, string>> Parameters { get; set; }
    }
}
