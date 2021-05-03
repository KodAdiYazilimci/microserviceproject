using MicroserviceProject.Infrastructure.Localization.Persistence.Configuration;
using MicroserviceProject.Infrastructure.Localization.Persistence.Entities;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Localization.Persistence.Repositories
{
    /// <summary>
    /// Dil çeviri repository sınıfı
    /// </summary>
    public class TranslationRepository
    {
        /// <summary>
        /// Veritabanı bağlantı sınıfı nesnesi
        /// </summary>
        private readonly TranslationDbContext translationDbContext;

        /// <summary>
        /// Dil çeviri repository sınıfı
        /// </summary>
        /// <param name="translationDbContext">Veritabanı bağlantı cümlesi</param>
        public TranslationRepository(TranslationDbContext translationDbContext) //: base(connectionString)
        {
            this.translationDbContext = translationDbContext;
        }

        /// <summary>
        /// Dil çevirilerini verir
        /// </summary>
        /// <returns></returns>
        public async Task<List<TranslationEntity>> GetTranslationsAsync(CancellationToken cancellationToken)
        {
            return
                await
                translationDbContext.Translations.Where(x => x.DeleteDate == null).ToListAsync(cancellationToken);
        }
    }
}
