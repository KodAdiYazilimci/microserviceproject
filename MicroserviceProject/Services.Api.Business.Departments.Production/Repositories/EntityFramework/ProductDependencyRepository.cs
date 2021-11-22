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
    /// Ürün bağımlılıkları tablosu için repository sınıfı
    /// </summary>
    public class ProductDependencyRepository : BaseRepository<ProductionContext, ProductDependencyEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[PRODUCTION_PRODUCT_DEPENDENCIES]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly ProductionContext _context;

        /// <summary>
        /// Ürün bağımlılıkları tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public ProductDependencyRepository(ProductionContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir ürün bağımlılığını siler
        /// </summary>
        /// <param name="id">Silinecek ürün bağımlılığı Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir ürün bağımlılığı kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek ürünün bağımlılığının Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            ProductDependencyEntity productDependency = await _context.ProductDependencies.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (productDependency != null)
            {
                productDependency.GetType().GetProperty(name).SetValue(productDependency, value);
            }
            else
            {
                throw new Exception("Ürün bağımlılığı kaydı bulunamadı");
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
            ProductDependencyEntity productDependency = await _context.ProductDependencies.FirstOrDefaultAsync(x => x.Id == id);

            if (productDependency != null)
            {
                productDependency.DeleteDate = null;
            }
            else
            {
                throw new Exception("Ürün bağımlılığı kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, ProductDependencyEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            ProductDependencyEntity product = await _context.ProductDependencies.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (product != null)
            {
                product.Amount = entity.Amount;
                product.DependedProductId = entity.DependedProductId;
                product.ProductId = entity.ProductId;
            }
            else
            {
                throw new Exception("Ürün bağımlılığı kaydı bulunamadı");
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
