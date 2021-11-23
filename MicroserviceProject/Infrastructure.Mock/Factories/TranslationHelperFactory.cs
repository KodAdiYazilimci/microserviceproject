using Infrastructure.Localization.Translation.Provider.Helpers;

namespace Infrastructure.Mock.Factories
{
    /// <summary>
    /// Dil çeviri yardımcısı sınıfını taklit eden sınıf
    /// </summary>
    public class TranslationHelperFactory
    {
        /// <summary>
        /// Dil çeviri yardımcısı
        /// </summary>
        private static TranslationHelper translationHelper;

        /// <summary>
        /// Dil çeviri yardımcısının örneğini verir
        /// </summary>
        public static TranslationHelper Instance
        {
            get
            {
                if (translationHelper == null)
                {
                    translationHelper = new TranslationHelper();
                }

                return translationHelper;
            }
        }
    }
}
