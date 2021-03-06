using MicroserviceProject.Services.Business.Departments.AA.Entities.Sql;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.AA.Repositories.Sql
{
    /// <summary>
    /// Envanter tablosu için repository sınıfı
    /// </summary>
    public class InventoryRepository : BaseRepository<InventoryEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[AA_INVENTORIES]";

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

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [NAME],
                                                      [CURRENT_STOCK_COUNT]
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
                    InventoryEntity inventory = new InventoryEntity
                    {
                        Id = sqlDataReader.GetInt32("ID"),
                        Name = sqlDataReader.GetString("NAME"),
                        CurrentStockCount = sqlDataReader.GetInt32("CURRENT_STOCK_COUNT")
                    };

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
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([NAME],
                                                      [CURRENT_STOCK_COUNT])
                                                      VALUES
                                                      (@NAME,
                                                       @CURRENT_STOCK_COUNT);
                                                      SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@NAME", ((object)inventory.Name) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@CURRENT_STOCK_COUNT", ((object)inventory.CurrentStockCount) ?? DBNull.Value);

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
        /// Belirli Id ye sahip envanterleri verir
        /// </summary>
        /// <param name="inventoryIds">Getirilecek envanterlerin Id değerleri</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryEntity>> GetForSpecificIdAsync(List<int> inventoryIds, CancellationToken cancellationToken)
        {
            List<InventoryEntity> inventories = new List<InventoryEntity>();

            string inQuery = inventoryIds.Any() ? string.Join(',', inventoryIds) : "0";

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [NAME],
                                                      [CURRENT_STOCK_COUNT]
                                                      FROM {TABLE_NAME}
                                                      WHERE 
                                                      DELETE_DATE IS NULL
                                                      AND
                                                      ID IN ({inQuery})",
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
                    InventoryEntity inventory = new InventoryEntity
                    {
                        Id = sqlDataReader.GetInt32("ID"),
                        Name = sqlDataReader.GetString("NAME"),
                        CurrentStockCount = sqlDataReader.GetInt32("CURRENT_STOCK_COUNT")
                    };

                    inventories.Add(inventory);
                }
            }

            return inventories;
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

        /// <summary>
        /// Envanterin stok adedini düşürür
        /// </summary>
        /// <param name="inventoryId">Envanterin Id değeri</param>
        /// <param name="count">Düşülecek stok miktarı</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> DescendStockCountAsync(int inventoryId, int count, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET CURRENT_STOCK_COUNT -= @VALUE
                                                      WHERE ID = @ID",
                                                                   UnitOfWork.SqlConnection,
                                                                   UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", inventoryId);
            sqlCommand.Parameters.AddWithValue("@VALUE", count);

            return await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        /// <summary>
        /// Envanterin stok adedini artırır
        /// </summary>
        /// <param name="inventoryId">Envanterin Id değeri</param>
        /// <param name="count">Artırılacak stok miktarı</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> IncreaseStockCountAsync(int inventoryId, int count, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET CURRENT_STOCK_COUNT += @VALUE
                                                      WHERE ID = @ID",
                                                             UnitOfWork.SqlConnection,
                                                             UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@ID", inventoryId);
            sqlCommand.Parameters.AddWithValue("@VALUE", count);

            return await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
