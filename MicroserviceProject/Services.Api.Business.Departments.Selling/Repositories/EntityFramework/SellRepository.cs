using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Selling.Configuration.Persistence;
using Services.Api.Business.Departments.Selling.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Selling.Repositories.EntityFramework
{
    /// <summary>
    /// Satışlar tablosu için repository sınıfı
    /// </summary>
    public class SellRepository : BaseRepository<SellingContext, SellEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[SELLING_SOLDS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly SellingContext _context;

        /// <summary>
        /// Satışlar tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public SellRepository(SellingContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir satışı siler
        /// </summary>
        /// <param name="id">Silinecek satışın Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir satış kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek satışın Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            SellEntity sellEntity = await _context.Sells.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (sellEntity != null)
            {
                sellEntity.GetType().GetProperty(name).SetValue(sellEntity, value);
            }
            else
            {
                throw new Exception("Satış kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir satış kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak satış kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            SellEntity sellEntity = await _context.Sells.FirstOrDefaultAsync(x => x.Id == id);

            if (sellEntity != null)
            {
                sellEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Satış kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, SellEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            SellEntity sell = await _context.Sells.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (sell != null)
            {
                sell.CustomerId = entity.CustomerId;
                sell.ProductId = entity.ProductId;
                sell.Quantity = entity.Quantity;
            }
            else
            {
                throw new Exception("Satış kaydı bulunamadı");
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
