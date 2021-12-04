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
        /// Çevirinin dil kodu
        /// </summary>
        public string LanguageCode { get; set; }
    }
}
