using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Selling.Entities.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Repositories.EntityFramework
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public abstract class BaseRepository<TContext, TEntity> : IAsyncDisposable where TContext : DbContext where TEntity : BaseEntity, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly TContext context;

        /// <summary>
        /// Repository sınıfları için temel sınıf
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public BaseRepository(TContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(TEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                await context.Set<TEntity>().AddAsync(entity, cancellationTokenSource.Token);
            }
        }

        public abstract Task UpdateAsync(int id, TEntity entity, CancellationTokenSource cancellationTokenSource);

        public async Task DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            if (!cancellationTokenSource.IsCancellationRequested)
            {
                TEntity entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

                context.Set<TEntity>().Remove(entity);
            }
        }

        public virtual async Task<TEntity> GetAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            return await GetAsQueryable().FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);
        }

        public virtual async Task<List<TEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            return await GetAsQueryable().ToListAsync(cancellationTokenSource.Token);
        }

        public virtual IQueryable<TEntity> GetAsQueryable()
        {
            return context.Set<TEntity>().Where(x => x.DeleteDate == null);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public virtual async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual async Task DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    await context.DisposeAsync();
                }

                disposed = true;
            }
        }
    }
}
