using MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql;
using MicroserviceProject.Services.Transaction;
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
    /// Maaş ödemesi tablosu için repository sınıfı
    /// </summary>
    public class SalaryPaymentRepository : BaseRepository<SalaryPaymentEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[ACCOUNTING_SALARY_PAYMENTS]";

        /// <summary>
        /// Maaş ödemesi tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public SalaryPaymentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Maaş ödemelerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<SalaryPaymentEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<SalaryPaymentEntity> salaryPayments = new List<SalaryPaymentEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT [ID],
                                                      [ACCOUNTING_BANK_ACCOUNTS_ID],
                                                      [ACCOUNTING_CURRENCIES_ID],
                                                      [DATE],
                                                      [AMOUNT]
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    SalaryPaymentEntity salaryPayment = new SalaryPaymentEntity();

                    salaryPayment.Id = sqlDataReader.GetInt32("ID");
                    salaryPayment.BankAccountId = sqlDataReader.GetInt32("ACCOUNTING_BANK_ACCOUNTS_ID");
                    salaryPayment.CurrencyId = sqlDataReader.GetInt32("ACCOUNTING_CURRENCIES_ID");
                    salaryPayment.Date = sqlDataReader.GetDateTime("DATE");
                    salaryPayment.Amount = sqlDataReader.GetDecimal("AMOUNT");

                    salaryPayments.Add(salaryPayment);
                }
            }

            return salaryPayments;
        }

        public async Task<List<SalaryPaymentEntity>> GetSalaryPaymentsOfWorkerAsync(int workerId, CancellationToken cancellationToken)
        {
            List<SalaryPaymentEntity> salaryPayments = new List<SalaryPaymentEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      SP.[ID],
                                                      SP.[ACCOUNTING_BANK_ACCOUNTS_ID],
                                                      SP.[ACCOUNTING_CURRENCIES_ID],
                                                      SP.[DATE],
                                                      SP.[AMOUNT]
                                                      FROM {TABLE_NAME} SP
                                                      INNER JOIN ACCOUNTING_BANK_ACCOUNTS BA
                                                      ON SP.ACCOUNTING_BANK_ACCOUNTS_ID = BA.ID
                                                      WHERE 
                                                      SP.DELETE_DATE IS NULL
                                                      AND
                                                      BA.HR_WORKERS_ID = @HR_WORKERS_ID",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerId) ?? DBNull.Value);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    SalaryPaymentEntity salaryPayment = new SalaryPaymentEntity();

                    salaryPayment.Id = sqlDataReader.GetInt32("ID");
                    salaryPayment.BankAccountId = sqlDataReader.GetInt32("ACCOUNTING_BANK_ACCOUNTS_ID");
                    salaryPayment.CurrencyId = sqlDataReader.GetInt32("ACCOUNTING_CURRENCIES_ID");
                    salaryPayment.Date = sqlDataReader.GetDateTime("DATE");
                    salaryPayment.Amount = sqlDataReader.GetDecimal("AMOUNT");

                    salaryPayments.Add(salaryPayment);
                }
            }

            return salaryPayments;
        }

        /// <summary>
        /// Yeni maaş ödemesi oluşturur
        /// </summary>
        /// <param name="salaryPayment">Oluşturulacak maaş ödemesi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(SalaryPaymentEntity salaryPayment, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([ACCOUNTING_BANK_ACCOUNTS_ID],
                                                      [ACCOUNTING_CURRENCIES_ID],
                                                      [DATE],
                                                      [AMOUNT])
                                                      VALUES
                                                      (@ACCOUNTING_BANK_ACCOUNTS_ID,
                                                      @ACCOUNTING_CURRENCIES_ID,
                                                      @DATE, 
                                                      @AMOUNT);
                                                      SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ACCOUNTING_BANK_ACCOUNTS_ID", ((object)salaryPayment.BankAccountId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@ACCOUNTING_CURRENCIES_ID", ((object)salaryPayment.CurrencyId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@DATE", ((object)salaryPayment.Date) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@AMOUNT", ((object)salaryPayment.Amount) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Disposed)
                {
                    UnitOfWork.Dispose();

                    Disposed = true;
                }
            }
        }

        /// <summary>
        /// Bir Id değerine sahip envanteri silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek envanterin Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = GETDATE()
                                                      WHERE ID = @ID",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir envanter kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak envanter kaydının Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = NULL
                                                      WHERE ID = @ID",
                                                              UnitOfWork.SqlConnection,
                                                              UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Bir envanter kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek envanterin Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET {name.ToUpper()} = @VALUE
                                                      WHERE ID = @ID",
                                                                  UnitOfWork.SqlConnection,
                                                                  UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@VALUE", value);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
