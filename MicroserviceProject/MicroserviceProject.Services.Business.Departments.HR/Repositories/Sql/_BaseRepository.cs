using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public abstract class BaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
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
    }
}
