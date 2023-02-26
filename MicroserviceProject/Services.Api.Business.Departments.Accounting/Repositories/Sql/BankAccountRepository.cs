using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.Accounting.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Accounting.Repositories.Sql
{
    /// <summary>
    /// Banka hesabı tablosu için repository sınıfı
    /// </summary>
    public class BankAccountRepository : BaseRepository<BankAccountEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>

        public const string TABLE_NAME = "[dbo].[ACCOUNTING_BANK_ACCOUNTS]";

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
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<BankAccountEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<BankAccountEntity> bankAccounts = new List<BankAccountEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT [ID],
                                                      [WORKERS_ID],
                                                      [IBAN],
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL",
                                                        UnitOfWork.SqlConnection,
                                                        UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
            {
                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
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
        }

        /// <summary>
        /// Bir çalışanın banka hesaplarının listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<BankAccountEntity>> GetBankAccountsAsync(int workerId, CancellationTokenSource cancellationTokenSource)
        {
            List<BankAccountEntity> bankAccounts = new List<BankAccountEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT [ID],
                                                      [HR_WORKERS_ID],
                                                      [IBAN]
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL
                                                      AND
                                                      HR_WORKERS_ID = @HR_WORKERS_ID",
                                                        UnitOfWork.SqlConnection,
                                                        UnitOfWork.SqlTransaction);

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerId) ?? DBNull.Value);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
            {
                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
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
        }

        /// <summary>
        /// Yeni banka hesabı oluşturur
        /// </summary>
        /// <param name="bankAccount">Oluşturulacak banka hesabı nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(BankAccountEntity bankAccount, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = UnitOfWork.SqlConnection.CreateCommand();

            sqlCommand.CommandText = $@"INSERT INTO {TABLE_NAME} ([WORKERS_ID],[IBAN]) VALUES (@HR_WORKERS_ID,@IBAN); SELECT CAST(scope_identity() AS int);";

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)bankAccount.WorkerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IBAN", ((object)bankAccount.IBAN) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    UnitOfWork.Dispose();

                    disposed = true;
                }
            }
        }

        /// <summary>
        /// Bir Id değerine sahip envanteri silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek envanterin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = GETDATE()
                                                      WHERE ID = @ID",
                                                      UnitOfWork.SqlConnection,
                                                      UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir envanter kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak envanter kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = NULL
                                                      WHERE ID = @ID",
                                                                 UnitOfWork.SqlConnection,
                                                                 UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Bir envanter kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek envanterin Id değeri</param>
        /// <param name="name">Değeri değiştirilecek kolonun adı</param>
        /// <param name="value">Yeni değer</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetAsync(int id, string name, object value, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET {name.ToUpper()} = @VALUE
                                                      WHERE ID = @ID",
                                                                    UnitOfWork.SqlConnection,
                                                                    UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@VALUE", value);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }
    }
}
