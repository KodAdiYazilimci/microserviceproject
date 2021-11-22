using Infrastructure.Transaction.Recovery;

using Microsoft.EntityFrameworkCore;

using Services.Api.Business.Departments.CR.Configuration.Persistence;
using Services.Api.Business.Departments.CR.Entities.EntityFramework;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.CR.Repositories.EntityFramework
{
    /// <summary>
    /// Müşteriler tablosu için repository sınıfı
    /// </summary>
    public class CustomerRepository : BaseRepository<CRContext, CustomerEntity>, IRollbackableDataAsync<int>, IAsyncDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[CR_CUSTOMERS]";

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        private readonly CRContext _context;

        /// <summary>
        /// Müşteriler tablosu için repository sınıfı
        /// </summary>
        /// <param name="context">Veritabanı bağlantı nesnesi</param>
        public CustomerRepository(CRContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Bir müşteriyi siler
        /// </summary>
        /// <param name="id">Silinecek müşterinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public new async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            await base.DeleteAsync(id, cancellationTokenSource);

            return id;
        }

        /// <summary>
        /// Bir müşteri kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek müşterinin Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            CustomerEntity customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id,cancellationTokenSource.Token);

            if (customerEntity != null)
            {
                customerEntity.GetType().GetProperty(name).SetValue(customerEntity, value);
            }
            else
            {
                throw new Exception("Müşteri kaydı bulunamadı");
            }

            return id;
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir müşteri kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak müşteri kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            CustomerEntity customerEntity = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customerEntity != null)
            {
                customerEntity.DeleteDate = null;
            }
            else
            {
                throw new Exception("Müşteri kaydı bulunamadı");
            }

            return id;
        }

        public override async Task UpdateAsync(int id, CustomerEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            CustomerEntity customer = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, cancellationTokenSource.Token);

            if (customer != null)
            {
                customer.IsLegal = entity.IsLegal;
                customer.MiddleName = entity.MiddleName;
                customer.Name = entity.Name;
                customer.Surname = entity.Surname;
            }
            else
            {
                throw new Exception("Müşteri kaydı bulunamadı");
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
