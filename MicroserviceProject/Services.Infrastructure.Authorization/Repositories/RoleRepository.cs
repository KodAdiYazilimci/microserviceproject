using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Infrastructure.Authorization.Configuration.Persistence;
using Services.Infrastructure.Authorization.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Repositories
{
    public class RoleRepository : BaseRepository<AuthContext, Role>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[ROLES]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly AuthContext _context;

        /// <summary>
        /// Rol repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public RoleRepository(AuthContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir rolü siler
        /// </summary>
        /// <param name="id">Silinecek rolün Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir rol kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek rolün Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            Role role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (role != null)
            {
                role.GetType().GetProperty(name).SetValue(role, value);
            }
            else
            {
                throw new Exception("Rol kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir rol kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak rol kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            Role role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role != null)
            {
                role.DeleteDate = null;
            }
            else
            {
                throw new Exception("Rol kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, Role entity, CancellationTokenSource cancellationTokenSource)
        {
            Role role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (role != null)
            {
                role.Name = entity.Name;
            }
            else
            {
                throw new Exception("Rol kaydı bulunamadı");
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
