using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork;
using Services.Business.Departments.AA.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.AA.Repositories.Sql
{
    /// <summary>
    /// Çalışan envanterleri tablosu için repository sınıfı
    /// </summary>
    public class WorkerInventoryRepository : BaseRepository<WorkerInventoryEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>

        public const string TABLE_NAME = "[dbo].[AA_WORKER_INVENTORIES]";

        /// <summary>
        /// Çalışan envanterleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public WorkerInventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Çalışan envanterlerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<WorkerInventoryEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<WorkerInventoryEntity> workerInventories = new List<WorkerInventoryEntity>();

            using (SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [HR_WORKERS_ID],
                                                      [AA_INVENTORIES_ID],
                                                      [FROM_DATE],
                                                      [TO_DATE]
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL",
                                                        UnitOfWork.SqlConnection,
                                                        UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                        {
                            WorkerInventoryEntity workerInventory = new WorkerInventoryEntity
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
                                sqlDataReader.GetDateTime("TO_DATE")
                            };

                            workerInventories.Add(workerInventory);
                        }
                    }

                    return workerInventories;
                }
            }
        }

        /// <summary>
        /// Yeni çalışan envanteri oluşturur
        /// </summary>
        /// <param name="workerInventory">Oluşturulacak çalışan envanteri nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(WorkerInventoryEntity workerInventory, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([HR_WORKERS_ID],
                                                      [AA_INVENTORIES_ID],
                                                      [FROM_DATE],
                                                      [TO_DATE])
                                                      VALUES
                                                      (@HR_WORKERS_ID,
                                                      @AA_INVENTORIES_ID,
                                                      @FROM_DATE,
                                                      @TO_DATE);
                                                      SELECT CAST(scope_identity() AS int)",
                                                      UnitOfWork.SqlConnection,
                                                      UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerInventory.WorkerId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@AA_INVENTORIES_ID", ((object)workerInventory.InventoryId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)workerInventory.FromDate) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)workerInventory.ToDate) ?? DBNull.Value);

                return (int)await sqlCommand.ExecuteScalarAsync(cancellationTokenSource.Token);
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
        /// Bir Id değerine sahip envanteri silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek envanterin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = GETDATE()
                                                      WHERE ID = @ID",
                                                      UnitOfWork.SqlConnection,
                                                      UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", id);

                return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Silindi olarak işaretlenmiş bir envanter kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak envanter kaydının Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> UnDeleteAsync(int id, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET DELETE_DATE = NULL
                                                      WHERE ID = @ID",
                                                               UnitOfWork.SqlConnection,
                                                               UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", id);

                return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
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
            using (SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET {name.ToUpper()} = @VALUE
                                                      WHERE ID = @ID",
                                                                     UnitOfWork.SqlConnection,
                                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", id);
                sqlCommand.Parameters.AddWithValue("@VALUE", value);

                return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
        }
    }
}
