
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Transaction.UnitOfWork.EntityFramework
{
    /// <summary>
    /// Entity Framework veritabanı işlemleri transaction için iş birimi arayüzü
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        Task SaveAsync(CancellationTokenSource cancellationTokenSource);
    }
}
