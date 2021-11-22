
using Microsoft.EntityFrameworkCore;

using Services.Api.Infrastructure.Authorization.Configuration.Persistence;
using Services.Api.Infrastructure.Authorization.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Infrastructure.Authorization.Repositories
{
    public class PolicyRoleRepository : BaseRepository<AuthContext, PolicyRole>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[POLICYROLES]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly AuthContext _context;

        /// <summary>
        /// Poliçe-rol repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public PolicyRoleRepository(AuthContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir poliçe-rolü siler
        /// </summary>
        /// <param name="id">Silinecek poliçe-rolün Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        public override async Task UpdateAsync(int id, PolicyRole entity, CancellationTokenSource cancellationTokenSource)
        {
            PolicyRole policyRole = await _context.PolicyRoles.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (policyRole != null)
            {
                policyRole.PolicyId = entity.PolicyId;
                policyRole.RoleId = entity.RoleId;
            }
            else
            {
                throw new Exception("Poliçe-rol kaydı bulunamadı");
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
