using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;
using Services.Api.Business.Departments.Buying.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Business.Departments.Buying.Repositories.Sql
{
    /// <summary>
    /// Envanter talepleri tablosu için repository sınıfı
    /// </summary>
    public class InventoryRequestRepository : BaseRepository<InventoryRequestEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[BUYING_INVENTORY_REQUESTS]";

        /// <summary>
        /// Envanter talepleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public InventoryRequestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Envanter taleplerinin listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<InventoryRequestEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<InventoryRequestEntity> inventoryRequests = new List<InventoryRequestEntity>();

            using (SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [INVENTORY_ID],
                                                      [HR_DEPARTMENTS_ID],
                                                      [AMOUNT],
                                                      [REVOKED],
                                                      [DONE]
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
                            InventoryRequestEntity inventoryRequest = new InventoryRequestEntity
                            {
                                Id = sqlDataReader.GetInt32("ID"),
                                InventoryId = sqlDataReader.GetInt32("INVENTORY_ID"),
                                DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                                Amount = sqlDataReader.GetInt32("AMOUNT"),
                                Revoked = sqlDataReader.GetBoolean("REVOKED"),
                                Done = sqlDataReader.GetBoolean("DONE")
                            };

                            inventoryRequests.Add(inventoryRequest);
                        }
                    }

                    return inventoryRequests;
                }
            }
        }

        /// <summary>
        /// Yeni envanter talebi oluşturur
        /// </summary>
        /// <param name="inventoryRequest">Oluşturulacak envanter talebi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(InventoryRequestEntity inventoryRequest, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([INVENTORY_ID],
                                                      [HR_DEPARTMENTS_ID],
                                                      [AMOUNT],
                                                      [REVOKED],
                                                      [DONE])
                                                      VALUES
                                                      (@INVENTORY_ID,
                                                      @HR_DEPARTMENTS_ID,
                                                      @AMOUNT,
                                                      @REVOKED,
                                                      @DONE);
                                                      SELECT CAST(scope_identity() AS int)",
                                                         UnitOfWork.SqlConnection,
                                                         UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@INVENTORY_ID", ((object)inventoryRequest.InventoryId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@HR_DEPARTMENTS_ID", ((object)inventoryRequest.DepartmentId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@AMOUNT", ((object)inventoryRequest.Amount) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@REVOKED", ((object)inventoryRequest.Revoked) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DONE", ((object)inventoryRequest.Done) ?? DBNull.Value);

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
        /// Belirli Id ye sahip envanter taleplerini verir
        /// </summary>
        /// <param name="inventoryRequestIds">Getirilecek envanter taleplerinin Id değerleri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<InventoryRequestEntity>> GetForSpecificIdAsync(List<int> inventoryRequestIds, CancellationTokenSource cancellationTokenSource)
        {
            List<InventoryRequestEntity> inventoryRequests = new List<InventoryRequestEntity>();

            string inQuery = inventoryRequestIds.Any() ? string.Join(',', inventoryRequestIds) : "0";

            using (SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [INVENTORY_ID],
                                                      [HR_DEPARTMENTS_ID],
                                                      [AMOUNT],
                                                      [REVOKED],
                                                      [DONE]
                                                      FROM {TABLE_NAME}
                                                      WHERE 
                                                      DELETE_DATE IS NULL
                                                      AND
                                                      ID IN ({inQuery})",
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
                            InventoryRequestEntity inventoryRequest = new InventoryRequestEntity
                            {
                                Id = sqlDataReader.GetInt32("ID"),
                                InventoryId = sqlDataReader.GetInt32("INVENTORY_ID"),
                                DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                                Amount = sqlDataReader.GetInt32("AMOUNT"),
                                Revoked = sqlDataReader.GetBoolean("REVOKED"),
                                Done = sqlDataReader.GetBoolean("DONE")
                            };

                            inventoryRequests.Add(inventoryRequest);
                        }
                    }

                    return inventoryRequests;
                }
            }
        }

        /// <summary>
        /// Envanter talebine ait satın alınma durumunu olumlu olarak tanımlar
        /// </summary>
        /// <param name="inventoryRequestId">Envanter talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> RevokeAsync(int inventoryRequestId, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET REVOKED = 1, DONE = 1
                                                      WHERE ID = @ID",
                                                         UnitOfWork.SqlConnection,
                                                         UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", inventoryRequestId);

                return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Envanter talebine ait satın alınma durumunu olumsuz olarak tanımlar
        /// </summary>
        /// <param name="inventoryRequestId"></param>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public async Task<int> UnRevokeAsync(int inventoryRequestId, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET REVOKED = 0, DONE = 1
                                                      WHERE ID = @ID",
                                                 UnitOfWork.SqlConnection,
                                                 UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", inventoryRequestId);

                return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
        }

        /// <summary>
        /// Bir Id değerine sahip envanter talebini silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek envanter talebinin Id değeri</param>
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
        /// Silindi olarak işaretlenmiş bir envanter talebi kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak envanter talebi kaydının Id değeri</param>
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
        /// Bir envanter talebi kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek envanter talebinin Id değeri</param>
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

        /// <summary>
        /// Envanter talebinin getirir
        /// </summary>
        /// <param name="inventoryRequestId">Getirilecek envanter talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<InventoryRequestEntity> GetAsync(int inventoryRequestId, CancellationTokenSource cancellationTokenSource)
        {
            InventoryRequestEntity inventoryRequestEntity = null;

            using (SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      TOP 1
                                                      [ID],
                                                      [INVENTORY_ID],
                                                      [HR_DEPARTMENTS_ID],
                                                      [AMOUNT],
                                                      [REVOKED],
                                                      [DONE]
                                                      FROM {TABLE_NAME}
                                                      WHERE 
                                                      ID = @ID
                                                      AND
                                                      DELETE_DATE IS NULL",
                                                      UnitOfWork.SqlConnection,
                                                      UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@ID", ((object)inventoryRequestId) ?? DBNull.Value);

                using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                {
                    if (sqlDataReader.HasRows)
                    {
                        while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                        {
                            inventoryRequestEntity = new InventoryRequestEntity
                            {
                                Id = sqlDataReader.GetInt32("ID"),
                                InventoryId = sqlDataReader.GetInt32("INVENTORY_ID"),
                                DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                                Amount = sqlDataReader.GetInt32("AMOUNT"),
                                Revoked = sqlDataReader.GetBoolean("REVOKED"),
                                Done = sqlDataReader.GetBoolean("DONE")
                            };
                        }
                    }

                    return inventoryRequestEntity;
                }
            }
        }
    }
}
