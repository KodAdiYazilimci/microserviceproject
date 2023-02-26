using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Api.Business.Departments.HR.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// İdari işler işlem öğeleri tablosu için repository sınıfı
    /// </summary>
    public class TransactionRepository : BaseRepository<RollbackEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[HR_TRANSACTIONS]";

        /// <summary>
        /// İdari işler işlem öğeleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public TransactionRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Yeni işlem kaydı oluşturur
        /// </summary>
        /// <param name="entity">Oluşturulacak işlem kaydı nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public override async Task<int> CreateAsync(RollbackEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([TRANSACTION_IDENTITY],
                                                       [TRANSACTION_TYPE],
                                                       [TRANSACTION_DATE],
                                                       [IS_ROLLED_BACK])
                                                      VALUES(
                                                       @TRANSACTION_IDENTITY,
                                                       @TRANSACTION_TYPE,
                                                       @TRANSACTION_DATE,
                                                       @IS_ROLLED_BACK);
                                                      SELECT CAST(scope_identity() AS int)",
                                              UnitOfWork.SqlConnection,
                                              UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@TRANSACTION_IDENTITY", ((object)entity.TransactionIdentity) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TRANSACTION_TYPE", ((object)entity.TransactionType) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TRANSACTION_DATE", ((object)entity.TransactionDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IS_ROLLED_BACK", ((object)entity.IsRolledback) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// İşlem listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<RollbackEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<RollbackEntity> entities = new List<RollbackEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [TRANSACTION_IDENTITY],
                                                      [TRANSACTION_TYPE],
                                                      [TRANSACTION_DATE],
                                                      [IS_ROLLED_BACK]
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
                        RollbackEntity inventory = new RollbackEntity();

                        inventory.Id = sqlDataReader.GetInt32("ID");
                        inventory.TransactionIdentity = sqlDataReader.GetString("TRANSACTION_IDENTITY");
                        inventory.TransactionType = sqlDataReader.GetInt32("TRANSACTION_TYPE");
                        inventory.TransactionDate = sqlDataReader.GetDateTime("TRANSACTION_DATE");
                        inventory.IsRolledback = sqlDataReader.GetBoolean("IS_ROLLED_BACK");

                        entities.Add(inventory);
                    }
                }

                return entities;
            }
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
        /// Bir işlemi geri alındı olarak işaretler
        /// </summary>
        /// <param name="transactionIdentity">Geri alındı olarak işaretlenecek işlemin kimliği</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetRolledbackAsync(string transactionIdentity, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET [IS_ROLLED_BACK] = 1
                                                      WHERE
                                                      [TRANSACTION_IDENTITY] = @TRANSACTION_IDENTITY",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@TRANSACTION_IDENTITY", transactionIdentity);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }
    }
}
