using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
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
    /// Envanter tablosu için repository sınıfı
    /// </summary>
    public class InventoryRepository : BaseRepository<InventoryEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Envanter tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public InventoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Envanterlerin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<InventoryEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<InventoryEntity> inventories = new List<InventoryEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [NAME]
                                                     FROM [dbo].[AA_INVENTORIES]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    InventoryEntity inventory = new InventoryEntity();

                    inventory.Id = sqlDataReader.GetInt32("ID");
                    inventory.Name = sqlDataReader.GetString("NAME");

                    inventories.Add(inventory);
                }
            }

            return inventories;
        }

        /// <summary>
        /// Yeni envanter oluşturur
        /// </summary>
        /// <param name="inventory">Oluşturulacak envanter nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(InventoryEntity inventory, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[AA_INVENTORIES]
                                                     ([NAME])
                                                     VALUES
                                                     (@NAME);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@NAME", ((object)inventory.Name) ?? DBNull.Value);

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

        /// <summary>
        /// Belirli Id ye sahip envanterleri verir
        /// </summary>
        /// <param name="inventoryIds">Getirilecek envanterlerin Id değerleri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryEntity>> GetForSpecificIdAsync(List<int> inventoryIds, CancellationToken cancellationToken)
        {
            List<InventoryEntity> inventories = new List<InventoryEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [NAME]
                                                     FROM [dbo].[AA_INVENTORIES]
                                                     WHERE 
                                                     DELETE_DATE IS NULL
                                                     AND
                                                     ID IN @IDS",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Parameters.AddWithValue("@IDS",((object)string.Join(',', inventoryIds)) ?? DBNull.Value);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    InventoryEntity inventory = new InventoryEntity();

                    inventory.Id = sqlDataReader.GetInt32("ID");
                    inventory.Name = sqlDataReader.GetString("NAME");

                    inventories.Add(inventory);
                }
            }

            return inventories;
        }
    }
}
