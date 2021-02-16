using MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Repositories.Sql
{
    /// <summary>
    /// Para birimleri tablosu için repository sınıfı
    /// </summary>
    public class CurrencyRepository : BaseRepository<CurrencyEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Para birimleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public CurrencyRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Para birimlerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<CurrencyEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<CurrencyEntity> currencies = new List<CurrencyEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [NAME],
                                                     [SHORT_NAME],
                                                     [DELETE_DATE]
                                                     FROM [ACCOUNTING_CURRENCIES]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    CurrencyEntity currency = new CurrencyEntity();

                    currency.Id = sqlDataReader.GetInt32("ID");
                    currency.Name = sqlDataReader.GetString("NAME");
                    currency.ShortName = sqlDataReader.GetString("SHORT_NAME");

                    currencies.Add(currency);
                }
            }

            return currencies;
        }

        /// <summary>
        /// Yeni Para birimi oluşturur
        /// </summary>
        /// <param name="currency">Oluşturulacak Para birimi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(CurrencyEntity currency, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [ACCOUNTING_CURRENCIES]
                                                     ([NAME],
                                                     [SHORT_NAME])
                                                     VALUES
                                                     (@NAME,
                                                      @SHORT_NAME);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@NAME", ((object)currency.Name) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@SHORT_NAME", ((object)currency.ShortName) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    UnitOfWork.Dispose();
                }

                disposed = true;
            }
        }
    }
}
