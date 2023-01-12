using Infrastructure.Logging.Model;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Abstraction
{
    /// <summary>
    /// Loglayıcı sınıfların arayüzü
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public interface IBulkLogger<TModel> : IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="models">Yazılacak logun modelleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        Task LogAsync(List<TModel> models, CancellationTokenSource cancellationTokenSource);
    }
}
