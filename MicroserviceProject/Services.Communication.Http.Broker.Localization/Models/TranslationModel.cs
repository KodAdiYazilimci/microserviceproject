using System.Collections.Generic;

namespace Services.Communication.Http.Broker.Localization.Models
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

        /// <summary>
        /// Parametreler
        /// </summary>
        public List<KeyValuePair<string, string>> Parameters { get; set; } = new List<KeyValuePair<string, string>>();
    }
}
