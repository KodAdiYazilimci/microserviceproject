using Infrastructure.Localization.Configuration;
using Infrastructure.Localization.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mock.Factories
{
    public class TranslationRepositoryFactory
    {
        private static TranslationRepository translationRepository;

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
