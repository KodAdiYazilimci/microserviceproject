using MicroserviceProject.Services.Business.Departments.Finance.Entities.Sql;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Finance.Repositories.Sql
{
    /// <summary>
    /// Karar verilen masraflar tablosu için repository sınıfı
    /// </summary>
    public class DecidedCostRepository : BaseRepository<DecidedCostEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[FINANCE_DECIDED_COSTS]";

        /// <summary>
        /// Karar verilen masraflar tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public DecidedCostRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Yeni masraf kararı oluşturur
        /// </summary>
        /// <param name="entity">Oluşturulacak masraf kararı nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        public override async Task<int> CreateAsync(DecidedCostEntity entity, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([BUYING_INVENTORY_REQUESTS_ID],
                                                      [APPROVED],
                                                      [DONE])
                                                      VALUES
                                                      (@BUYING_INVENTORY_REQUESTS_ID,
                                                       @APPROVED,
                                                       @DONE);
                                                       SELECT CAST(scope_identity() AS int)",
                                              UnitOfWork.SqlConnection,
                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@BUYING_INVENTORY_REQUESTS_ID", ((object)entity.InventoryRequestId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@APPROVED", ((object)entity.Approved) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@DONE", ((object)entity.Done) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// Masraf kararı listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<DecidedCostEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<DecidedCostEntity> entities = new List<DecidedCostEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [BUYING_INVENTORY_REQUESTS_ID],
                                                      [APPROVED],
                                                      [DONE]
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    DecidedCostEntity inventory = new DecidedCostEntity
                    {
                        Id = sqlDataReader.GetInt32("ID"),
                        InventoryRequestId = sqlDataReader.GetInt32("BUYING_INVENTORY_REQUESTS_ID"),
                        Approved = sqlDataReader.GetBoolean("APPROVED"),
                        Done = sqlDataReader.GetBoolean("DONE")
                    };

                    entities.Add(inventory);
                }
            }

            return entities;
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
                }

                disposed = true;

                Dispose();
            }
        }

        /// <summary>
        /// Bir işlemi geri alındı olarak işaretler
        /// </summary>
        /// <param name="transactionIdentity">Geri alındı olarak işaretlenecek işlemin kimliği</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetRolledbackAsync(string transactionIdentity, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET [IS_ROLLED_BACK] = 1
                                                      WHERE
                                                      [TRANSACTION_IDENTITY] = @TRANSACTION_IDENTITY",
                                               UnitOfWork.SqlConnection,
                                               UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@TRANSACTION_IDENTITY", transactionIdentity);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Bir Id değerine sahip envanter talebini silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek envanter talebinin Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = GETDATE()
                                                      WHERE ID = @ID",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir envanter talebi kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak envanter talebi kaydının Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = NULL
                                                      WHERE ID = @ID",
                                                              UnitOfWork.SqlConnection,
                                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Bir envanter talebi kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek envanter talebinin Id değeri</param>
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
                                                                  UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", id);
            sqlCommand.Parameters.AddWithValue("@VALUE", value);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
