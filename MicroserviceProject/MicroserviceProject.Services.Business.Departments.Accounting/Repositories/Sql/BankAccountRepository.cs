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
    /// Banka hesabı tablosu için repository sınıfı
    /// </summary>
    public class BankAccountRepository : BaseRepository<BankAccountEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Banka hesabı tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public BankAccountRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Banka hesaplarının listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<BankAccountEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<BankAccountEntity> bankAccounts = new List<BankAccountEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT [ID],
                                                     [WORKERS_ID],
                                                     [IBAN],
                                                     FROM [ACCOUNTING_BANK_ACCOUNTS]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    BankAccountEntity bankAccount = new BankAccountEntity();

                    bankAccount.Id = sqlDataReader.GetInt32("ID");
                    bankAccount.WorkerId = sqlDataReader.GetInt32("HR_WORKERS_ID");
                    bankAccount.IBAN = sqlDataReader.GetString("IBAN");

                    bankAccounts.Add(bankAccount);
                }
            }

            return bankAccounts;
        }

        /// <summary>
        /// Bir çalışanın banka hesaplarının listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<BankAccountEntity>> GetBankAccountsAsync(int workerId, CancellationToken cancellationToken)
        {
            List<BankAccountEntity> bankAccounts = new List<BankAccountEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT [ID],
                                                     [HR_WORKERS_ID],
                                                     [IBAN]
                                                     FROM [ACCOUNTING_BANK_ACCOUNTS]
                                                     WHERE DELETE_DATE IS NULL
                                                     AND
                                                     HR_WORKERS_ID = @HR_WORKERS_ID",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerId) ?? DBNull.Value);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    BankAccountEntity bankAccount = new BankAccountEntity();

                    bankAccount.Id = sqlDataReader.GetInt32("ID");
                    bankAccount.WorkerId = sqlDataReader.GetInt32("HR_WORKERS_ID");
                    bankAccount.IBAN = sqlDataReader.GetString("IBAN");

                    bankAccounts.Add(bankAccount);
                }
            }

            return bankAccounts;
        }

        /// <summary>
        /// Yeni banka hesabı oluşturur
        /// </summary>
        /// <param name="bankAccount">Oluşturulacak banka hesabı nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(BankAccountEntity bankAccount, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [ACCOUNTING_BANK_ACCOUNTS]
                                                    ([WORKERS_ID],
                                                     [IBAN])
                                                     VALUES
                                                    (@HR_WORKERS_ID,
                                                     @IBAN);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)bankAccount.WorkerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IBAN", ((object)bankAccount.IBAN) ?? DBNull.Value);

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
