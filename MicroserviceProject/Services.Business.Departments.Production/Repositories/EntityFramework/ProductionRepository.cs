using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Business.Departments.Production.Configuration.Persistence;
using Services.Business.Departments.Production.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Production.Repositories.EntityFramework
{
    /// <summary>
    /// Üretilen ürünler tablosu için repository sınıfı
    /// </summary>
    public class ProductionRepository : BaseRepository<ProductionContext, ProductionEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[PRODUCTION_PRODUCTIONS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly ProductionContext _context;

        /// <summary>
        /// Üretilen ürünler tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public ProductionRepository(ProductionContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir üretilen ürünü siler
        /// </summary>
        /// <param name="id">Silinecek üretilen ürünün Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir üretilen ürün kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek üretilen ürünün Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            ProductionEntity productionEntity = await _context.Productions.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (productionEntity != null)
            {
                productionEntity.GetType().GetProperty(name).SetValue(productionEntity, value);
            }
            else
            {
                throw new Exception("Üretilen ürün kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir üretilen ürün kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak üretilen ürün kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            ProductionEntity productionEntity = await _context.Productions.FirstOrDefaultAsync(x => x.Id == id);

            if (productionEntity != null)
            {
                productionEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Üretilen ürün kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, ProductionEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            ProductionEntity production = await _context.Productions.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (production != null)
            {
                production.ProductId = entity.ProductId;
                production.DepartmentId = entity.DepartmentId;
                production.ReferenceNumber = entity.ReferenceNumber;
                production.StatusId = entity.StatusId;
                production.RequestedAmount = entity.RequestedAmount;
            }
            else
            {
                throw new Exception("Üretilen ürün kaydı bulunamadı");
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
