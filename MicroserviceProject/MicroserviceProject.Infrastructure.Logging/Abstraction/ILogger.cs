using MicroserviceProject.Infrastructure.Logging.Model;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.Abstraction
{
    /// <summary>
    /// Loglayıcı sınıfların arayüzü
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public interface ILogger<TModel> : IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        Task LogAsync(TModel model, CancellationToken cancellationToken);
    }
}
