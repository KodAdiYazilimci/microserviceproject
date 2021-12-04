﻿using Infrastructure.Localization.Translation.Persistence.EntityFramework.Persistence;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories;

namespace Infrastructure.Localization.Translation.Persistence.EntityFramework.Mock.Persistence
{
    /// <summary>
    /// Dil çeviri repository sınıfını taklit eden sınıf
    /// </summary>
    public class TranslationRepositoryFactory
    {
        /// <summary>
        /// Dil çeviri repository nesnesi
        /// </summary>
        private static TranslationRepository translationRepository;

        /// <summary>
        /// Dil çeviri repository nesnesini verir
        /// </summary>
        /// <param name="translationDbContext">Dil çeviri context sınıfı nesnesi</param>
        /// <returns></returns>
        public static TranslationRepository GetTranslationRepository(TranslationDbContext translationDbContext)
        {
            if (translationRepository == null)
            {
                translationRepository = new TranslationRepository(translationDbContext);
            }

            return translationRepository;
        }
    }
}