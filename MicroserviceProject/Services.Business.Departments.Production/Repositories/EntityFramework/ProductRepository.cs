using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Business.Departments.Production.Configuration.Persistence;
using Services.Business.Departments.Production.Entities.EntityFramework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Repositories.EntityFramework
{
    /// <summary>
    /// Ürünler tablosu için repository sınıfı
    /// </summary>
    public class ProductRepository : BaseRepository<ProductionContext, ProductEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[PRODUCTION_PRODUCTS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly ProductionContext _context;

        /// <summary>
        /// Ürünler tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public ProductRepository(ProductionContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir ürünü siler
        /// </summary>
        /// <param name="id">Silinecek ürünün Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir ürün kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek ürünün Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (productEntity != null)
            {
                productEntity.GetType().GetProperty(name).SetValue(productEntity, value);
            }
            else
            {
                throw new Exception("Ürün kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir ürün kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak ürün kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity productEntity = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productEntity != null)
            {
                productEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Ürün kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, ProductEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            ProductEntity product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (product != null)
            {
                product.ProductId = entity.ProductId;
                product.ProductName = entity.ProductName;
            }
            else
            {
                throw new Exception("Ürün kaydı bulunamadı");
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
