using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql
{
    /// <summary>
    /// Çalışan envanterleri tablosu için repository sınıfı
    /// </summary>
    public class WorkerInventoryRepository : BaseRepository<WorkerInventoryEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<WorkerInventoryEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<WorkerInventoryEntity> workerInventories = new List<WorkerInventoryEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [HR_WORKERS_ID],
                                                     [AA_INVENTORIES_ID],
                                                     [FROM_DATE],
                                                     [TO_DATE]
                                                     FROM [dbo].[AA_WORKER_INVENTORIES]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    WorkerInventoryEntity workerInventory = new WorkerInventoryEntity();

                    workerInventory.Id = sqlDataReader.GetInt32("ID");
                    workerInventory.WorkerId = sqlDataReader.GetInt32("HR_WORKERS_ID");
                    workerInventory.InventoryId = sqlDataReader.GetInt32("AA_INVENTORIES_ID");
                    workerInventory.FromDate = sqlDataReader.GetDateTime("FROM_DATE");
                    workerInventory.ToDate = sqlDataReader.GetDateTime("TO_DATE");

                    workerInventories.Add(workerInventory);
                }
            }

            return workerInventories;
        }

        /// <summary>
        /// Yeni çalışan envanteri oluşturur
        /// </summary>
        /// <param name="workerInventory">Oluşturulacak çalışan envanteri nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(WorkerInventoryEntity workerInventory, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[AA_WORKER_INVENTORIES]
                                                     ([HR_WORKERS_ID],
                                                     [AA_INVENTORIES_ID],
                                                     [FROM_DATE],
                                                     [TO_DATE])
                                                     VALUES
                                                     (@HR_WORKERS_ID,
                                                      @AA_INVENTORIES_ID,
                                                      @FROM_DATE,
                                                      @TO_DATE)
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerInventory.WorkerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@AA_INVENTORIES_ID", ((object)workerInventory.InventoryId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)workerInventory.FromDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)workerInventory.ToDate) ?? DBNull.Value);

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
