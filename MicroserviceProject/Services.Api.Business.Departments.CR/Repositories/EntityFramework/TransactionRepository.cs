
using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.CR.Configuration.Persistence;
using Services.Api.Business.Departments.CR.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Repositories.EntityFramework
{
    /// <summary>
    /// Müşteri ilişkileri işlem öğeleri tablosu için repository sınıfı
    /// </summary>
    public class TransactionRepository : BaseRepository<CRContext, RollbackEntity>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly CRContext _context;

        /// <summary>
        /// Müşteri ilişkileri işlem öğeleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public TransactionRepository(CRContext context) : base(context)
        {
            _context = context;
        }
   
        public override async Task UpdateAsync(int id, RollbackEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            RollbackEntity rollbackEntity = await _context.Rollbacks.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (rollbackEntity != null)
            {
                rollbackEntity.IsRolledback = entity.IsRolledback;
                rollbackEntity.TransactionIdentity = entity.TransactionIdentity;
            }
            else
            {
                throw new Exception("Transaction bulunamadı");
            }
        }

        /// <summary>
        /// Bir işlemi geri alındı olarak işaretler
        /// </summary>
        /// <param name="transactionIdentity">Geri alındı olarak işaretlenecek işlemin kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<string> SetRolledbackAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            RollbackEntity rollbackEntity = await _context.Rollbacks.FirstOrDefaultAsync(x => x.TransactionIdentity == transactionIdentity, cancellationTokenSource.Token);

            if (rollbackEntity != null)
            {
                rollbackEntity.IsRolledback = true;
            }
            else
            {
                throw new Exception("Transaction bulunamadı");
            }

            return transactionIdentity;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <returns></returns>
        public override async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);

            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    await _context.DisposeAsync();

                    disposed = true;
                }
            }
        }
    }
}
