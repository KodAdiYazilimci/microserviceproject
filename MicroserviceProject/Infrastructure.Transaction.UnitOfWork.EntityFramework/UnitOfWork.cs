
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Transaction.UnitOfWork.EntityFramework
{
    /// <summary>
    /// Entity Framework veritabanı işlemleri transaction için iş birimi sınıfı
    /// </summary>
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Transactionın tamamlanıp tamamlanmadığı bilgisi
        /// </summary>
        private bool hasCommittedTransaction = false;

        /// <summary>
        /// DbContextten türemiş sınıfın nesnesi
        /// </summary>
        private DbContext dbContext { get; set; }

        /// <summary>
        /// Entity Framework veritabanı işlemleri transaction için iş birimi sınıfı
        /// </summary>
        /// <param name="dbContext">DbContextten türemiş sınıfın nesnesi</param>
        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;

            this.dbContext.Database.BeginTransaction();
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        private async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (!hasCommittedTransaction)
                    {
                        await SaveAsync(null);
                    }

                    await dbContext.DisposeAsync();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task SaveAsync(CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;

            try
            {
                await dbContext.Database.CommitTransactionAsync(cancellationTokenSource != null ? cancellationTokenSource.Token : default(CancellationToken));
            }
            catch (Exception ex)
            {
                exception = ex;

                await dbContext.Database.RollbackTransactionAsync(cancellationTokenSource != null ? cancellationTokenSource.Token : default(CancellationToken));
            }
            finally
            {
                hasCommittedTransaction = true;
            }

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
