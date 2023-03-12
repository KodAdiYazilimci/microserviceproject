using Infrastructure.Localization.Translation.Models;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Localization.Translation.Persistence.Abstract
{
    /// <summary>
    /// Dil çeviri repository arayüzü
    /// </summary>
    public interface ITranslationRepository : IDisposable
    {
        List<TranslationModel> GetTranslations();


        /// <summary>
        /// Dil çevirilerini verir
        /// </summary>
        /// <returns></returns>
        Task<List<TranslationModel>> GetTranslationsAsync(CancellationToken cancellationToken);
    }
}
