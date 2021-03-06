using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public abstract class BaseRepository<TEntity> : IDisposable where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        protected bool Disposed = false;

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

        public abstract Task<int> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        public abstract Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken);

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
                if (!Disposed)
                {
                    if (UnitOfWork != null)
                    {
                        UnitOfWork.Dispose();
                    }
                }

                Disposed = true;
            }
        }
    }
}
