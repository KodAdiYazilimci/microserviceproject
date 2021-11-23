using Infrastructure.Localization.Translation.Models;
using Infrastructure.Localization.Translation.Persistence.Abstract;
using Infrastructure.Localization.Translation.Persistence.EntityFramework.Persistence;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Localization.Translation.Persistence.EntityFramework.Repositories
{
    /// <summary>
    /// Dil çeviri repository sınıfı
    /// </summary>
    public class TranslationRepository : ITranslationRepository, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı bağlantı sınıfı nesnesi
        /// </summary>
        private TranslationDbContext translationDbContext;

        /// <summary>
        /// Dil çeviri repository sınıfı
        /// </summary>
        /// <param name="translationDbContext">Veritabanı bağlantı cümlesi</param>
        public TranslationRepository(TranslationDbContext translationDbContext)
        {
            this.translationDbContext = translationDbContext;
        }

        /// <summary>
        /// Dil çevirilerini verir
        /// </summary>
        /// <returns></returns>
        public List<TranslationModel> GetTranslations()
        {
            return
                translationDbContext
                .Translations
                .Where(x => x.DeleteDate == null)
                .Select(x => new TranslationModel()
                {
                    Key = x.Key,
                    LanguageCode = x.LanguageCode,
                    Text = x.Text
                })
                .ToList();
        }

        /// <summary>
        /// Dil çevirilerini verir
        /// </summary>
        /// <returns></returns>
        public async Task<List<TranslationModel>> GetTranslationsAsync(CancellationToken cancellationToken)
        {
            return
                await
                translationDbContext
                .Translations
                .Where(x => x.DeleteDate == null)
                .Select(x => new TranslationModel()
                {
                    Key = x.Key,
                    Text = x.Text,
                    LanguageCode = x.LanguageCode
                })
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    translationDbContext?.Dispose();
                    translationDbContext = null;
                }

                disposed = true;
            }
        }
    }
}
