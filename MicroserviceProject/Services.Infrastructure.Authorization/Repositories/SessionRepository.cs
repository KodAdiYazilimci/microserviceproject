
using Microsoft.EntityFrameworkCore;

using Services.Infrastructure.Authorization.Configuration.Persistence;
using Services.Infrastructure.Authorization.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Repositories
{
    /// <summary>
    /// Oturum repository sınıfı
    /// </summary>
    public class SessionRepository : BaseRepository<AuthContext, Session>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[SESSIONS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly AuthContext _context;

        /// <summary>
        /// Oturum repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public SessionRepository(AuthContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir oturumu siler
        /// </summary>
        /// <param name="id">Silinecek oturumun Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        public override async Task UpdateAsync(int id, Session entity, CancellationTokenSource cancellationTokenSource)
        {
            Session session = await _context.Sessions.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (session != null)
            {
                session.IpAddress = entity.IpAddress;
                session.IsValid = entity.IsValid;
                session.Token = entity.Token;
                session.UserAgent = entity.UserAgent;
                session.UserId = entity.UserId;
                session.ValidTo = entity.ValidTo;
                session.Region = entity.Region;
            }
            else
            {
                throw new Exception("Oturum kaydı bulunamadı");
            }
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
