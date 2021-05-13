using Infrastructure.Localization.Helpers;

namespace Infrastructure.Mock.Factories
{
    public class TranslationHelperFactory
    {
        private static TranslationHelper translationHelper;

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
