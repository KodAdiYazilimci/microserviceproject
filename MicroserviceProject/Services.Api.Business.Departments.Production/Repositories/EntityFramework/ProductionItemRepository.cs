using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.Production.Configuration.Persistence;
using Services.Api.Business.Departments.Production.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Production.Repositories.EntityFramework
{
    /// <summary>
    /// Üretilen ürünler tablosu için detay repository sınıfı
    /// </summary>
    public class ProductionItemRepository : BaseRepository<ProductionContext, ProductionItemEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[PRODUCTION_PRODUCTIONS_ITEMS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly ProductionContext _context;

        /// <summary>
        /// Üretilen ürünler tablosu için detay repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public ProductionItemRepository(ProductionContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir üretilen ürün detayını siler
        /// </summary>
        /// <param name="id">Silinecek üretilen ürün detayının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir üretilen ürün detay kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek üretilen ürünün detay Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            ProductionItemEntity productionItemEntity = await _context.ProductionItems.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (productionItemEntity != null)
            {
                productionItemEntity.GetType().GetProperty(name).SetValue(productionItemEntity, value);
            }
            else
            {
                throw new Exception("Üretilen ürün detay kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir üretilen ürün detay kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak üretilen ürün detay kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            ProductionItemEntity productionEntity = await _context.ProductionItems.FirstOrDefaultAsync(x => x.Id == id);

            if (productionEntity != null)
            {
                productionEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Üretilen ürün detay kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, ProductionItemEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            ProductionItemEntity productionItem = await _context.ProductionItems.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (productionItem != null)
            {
                productionItem.DependedProductId = entity.DependedProductId;
                productionItem.StatusId = entity.StatusId;
                productionItem.RequiredAmount = entity.RequiredAmount;
            }
            else
            {
                throw new Exception("Üretilen ürün detay kaydı bulunamadı");
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
