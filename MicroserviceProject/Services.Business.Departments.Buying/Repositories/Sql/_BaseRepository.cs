using Infrastructure.Transaction.UnitOfWork.Sql;
using Services.Business.Departments.Buying.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Buying.Repositories.Sql
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public abstract class BaseRepository<TEntity> : IDisposable where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı işlemlerini kapsayan iş birimi nesnesi
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Repository sınıfları için temel sınıf
        /// </summary>
        /// <param name="configuration">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public BaseRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract Task<int> CreateAsync(TEntity entity, CancellationTokenSource cancellationTokenSource);
        public abstract Task<List<TEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource);

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
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    UnitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
