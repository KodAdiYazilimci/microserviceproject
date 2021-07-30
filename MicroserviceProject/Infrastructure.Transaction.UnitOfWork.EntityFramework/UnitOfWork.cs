
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;

using System;
using System.Collections.Generic;
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
        /// DbContextten türemiş sınıfın nesnesi
        /// </summary>
        private DbContext dbContext { get; set; }

        /// <summary>
        /// Transaction tamamlanmadan önce entitylerin yapısını değiştirecek handler
        /// </summary>
        /// <param name="entityEntries"></param>
        public delegate void EditEntriesBeforeCommitHandler(IEnumerable<EntityEntry> entityEntries);

        /// <summary>
        /// Transaction tamamlanmadan önce entitylerin yapısını değiştirir
        /// </summary>
        public EditEntriesBeforeCommitHandler EditEntriesBeforeCommit { get; set; }

        /// <summary>
        /// Entity Framework veritabanı işlemleri transaction için iş birimi sınıfı
        /// </summary>
        /// <param name="dbContext">DbContextten türemiş sınıfın nesnesi</param>
        public UnitOfWork(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task SaveAsync(CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;

            using (IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync(cancellationTokenSource.Token))
            {
                try
                {
                    if (EditEntriesBeforeCommit != null)
                    {
                        EditEntriesBeforeCommit(dbContext.ChangeTracker.Entries());
                    }

                    int result = await dbContext.SaveChangesAsync(cancellationTokenSource != null ? cancellationTokenSource.Token : default(CancellationToken));

                    await transaction.CommitAsync(cancellationTokenSource != null ? cancellationTokenSource.Token : default(CancellationToken));
                }
                catch (Exception ex)
                {
                    exception = ex;

                    await transaction.RollbackAsync(cancellationTokenSource != null ? cancellationTokenSource.Token : default(CancellationToken));
                }
            }

            if (exception != null)
            {
                throw exception;
            }
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
                    await dbContext.DisposeAsync();
                }

                disposed = true;
            }
        }
    }
}
