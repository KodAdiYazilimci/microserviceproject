using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.UnitOfWork
{
    /// <summary>
    /// Ms SQL veritabanı işlemleri transaction için iş birimi arayüzü
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// Sql bütünlük yönetiminden sorumlu transaction nesnesi
        /// </summary>
        SqlTransaction SqlTransaction { get; }

        /// <summary>
        /// Veritabanı bağlantı nesnesi
        /// </summary>
        SqlConnection SqlConnection { get; }

        /// <summary>
        /// Veritabanı işlem bütünlüğünü çalıştırır
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
