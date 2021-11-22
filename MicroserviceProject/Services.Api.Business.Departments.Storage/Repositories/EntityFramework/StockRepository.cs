using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Storage.Configuration.Persistence;
using Services.Api.Business.Departments.Storage.Entities.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Storage.Repositories.EntityFramework
{
    /// <summary>
    /// Stoklar tablosu için repository sınıfı
    /// </summary>
    public class StockRepository : BaseRepository<StorageContext, StockEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[STORAGE_STOCKS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly StorageContext _context;

        /// <summary>
        /// Stoklar tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public StockRepository(StorageContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir stoğu siler
        /// </summary>
        /// <param name="id">Silinecek stoğun Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir stok kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek stoğun Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            StockEntity stockEntity = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (stockEntity != null)
            {
                stockEntity.GetType().GetProperty(name).SetValue(stockEntity, value);
            }
            else
            {
                throw new Exception("Stok kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir stok kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak stok kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            StockEntity stockEntity = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stockEntity != null)
            {
                stockEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Stok kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, StockEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            StockEntity stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (stock != null)
            {
                stock.ProductId = entity.ProductId;
                stock.Amount = entity.Amount;
            }
            else
            {
                throw new Exception("Stok kaydı bulunamadı");
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
