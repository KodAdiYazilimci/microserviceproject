using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql
{
    /// <summary>
    /// Çalışanlara verilecek stoğu olmayan envanterler tablosu için repository sınıfı
    /// </summary>
    public class PendingWorkerInventoryRepository : BaseRepository<PendingWorkerInventoryEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>

        public const string TABLE_NAME = "[dbo].[AA_WORKER_PENDING_INVENTORIES]";

        /// <summary>
        /// Çalışanlara verilecek stoğu olmayan envanterler tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public PendingWorkerInventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Çalışanlara verilecek stoğu olmayan envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<PendingWorkerInventoryEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<PendingWorkerInventoryEntity> workerInventories = new List<PendingWorkerInventoryEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [HR_WORKERS_ID],
                                                      [AA_INVENTORIES_ID],
                                                      [FROM_DATE],
                                                      [TO_DATE],
                                                      [STOCK_COUNT],
                                                      [IS_COMPLETE]
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
                    PendingWorkerInventoryEntity workerInventory = new PendingWorkerInventoryEntity
                    {
                        Id = sqlDataReader.GetInt32("ID"),
                        WorkerId = sqlDataReader.GetInt32("HR_WORKERS_ID"),
                        InventoryId = sqlDataReader.GetInt32("AA_INVENTORIES_ID"),
                        FromDate = sqlDataReader.GetDateTime("FROM_DATE"),
                        ToDate =
                        sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TO_DATE"))
                        ?
                        null
                        :
                        sqlDataReader.GetDateTime("TO_DATE"),
                        StockCount = sqlDataReader.GetInt32("STOCK_COUNT"),
                        IsComplete = sqlDataReader.GetBoolean("IS_COMPLETE")
                    };

                    workerInventories.Add(workerInventory);
                }
            }

            return workerInventories;
        }

        /// <summary>
        /// Çalışanlara verilecek stoğu olmayan envanter kaydı oluşturur
        /// </summary>
        /// <param name="pendingWorkerInventory">Oluşturulacak çalışan envanteri nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(PendingWorkerInventoryEntity pendingWorkerInventory, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([HR_WORKERS_ID],
                                                      [AA_INVENTORIES_ID],
                                                      [FROM_DATE],
                                                      [TO_DATE],
                                                      [STOCK_COUNT],
                                                      [IS_COMPLETE])
                                                      VALUES
                                                      (@HR_WORKERS_ID,
                                                       @AA_INVENTORIES_ID,
                                                       @FROM_DATE,
                                                       @TO_DATE,
                                                       @STOCK_COUNT,
                                                       @IS_COMPLETE);
                                                       SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)pendingWorkerInventory.WorkerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@AA_INVENTORIES_ID", ((object)pendingWorkerInventory.InventoryId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)pendingWorkerInventory.FromDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)pendingWorkerInventory.ToDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@STOCK_COUNT", ((object)pendingWorkerInventory.StockCount) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IS_COMPLETE", ((object)pendingWorkerInventory.IsComplete) ?? DBNull.Value);

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
                if (!disposed)
                {
                    UnitOfWork.Dispose();
                }

                disposed = true;

                Dispose();
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
                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", id);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Bekleyen çalışan envanterini ileriki tarihe erteler
        /// </summary>
        /// <param name="workerId">Çalışanın Id değeri</param>
        /// <param name="inventoryId">Envanterin Id değeri</param>
        /// <param name="toDate">Ertelenecek tarih</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DelayAsync(int workerId, int inventoryId, DateTime toDate, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET IS_COMPLETE = 0, NEXT_REQUEST_DATE = @NEXT_DATE
                                                      WHERE 
                                                      HR_WORKERS_ID = @WORKER_ID
                                                      AND
                                                      AA_INVENTORIES_ID = @INVENTORY_ID
                                                      AND
                                                      IS_COMPLETE = 0",
                                                              UnitOfWork.SqlConnection,
                                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@WORKER_ID", workerId);
            sqlCommand.Parameters.AddWithValue("@INVENTORY_ID", inventoryId);
            sqlCommand.Parameters.AddWithValue("@NEXT_DATE", toDate);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Bekleyen çalışan envanterini tamamlandı olarak işaretler
        /// </summary>
        /// <param name="workerId">Çalışanın Id değeri</param>
        /// <param name="inventoryId">Envanterin Id değeri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> SetCompleteAsync(int workerId, int inventoryId, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET IS_COMPLETE = 1, NEXT_REQUEST_DATE = NULL
                                                      WHERE 
                                                      HR_WORKERS_ID = @WORKER_ID
                                                      AND
                                                      AA_INVENTORIES_ID = @INVENTORY_ID
                                                      AND
                                                      IS_COMPLETE = 0",
                                                              UnitOfWork.SqlConnection,
                                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@WORKER_ID", workerId);
            sqlCommand.Parameters.AddWithValue("@INVENTORY_ID", inventoryId);

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
                                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

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
