using MicroserviceProject.Services.Transaction.Models;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Transaction
{
    /// <summary>
    /// Bir veri seti için transaction işlemleri arayüzü
    /// </summary>
    /// <typeparam name="TIdentity">İşlemin geri dönüş tipi</typeparam>
    public interface IRollbackableAsync<TIdentity>
    {
        /// <summary>
        /// İşlem yığınını geri almak için yedekleme noktası oluşturur
        /// </summary>
        /// <param name="rollback">Yedeklemenin modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        Task<TIdentity> CreateCheckpointAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource);

        /// <summary>
        /// Bir işlemi veri setinden geri alır
        /// </summary>
        /// <param name="rollback">Geri alınacak işlemin yedekleme modeli</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns>TIdentity işlemin geri dönüş tipidir</returns>
        Task<TIdentity> RollbackTransactionAsync(RollbackModel rollback, CancellationTokenSource cancellationTokenSource);
    }
}
