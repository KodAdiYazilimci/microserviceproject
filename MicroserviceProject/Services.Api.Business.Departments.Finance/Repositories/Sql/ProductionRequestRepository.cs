using Infrastructure.Transaction.Recovery;
using Infrastructure.Transaction.UnitOfWork.Sql;

using Services.Business.Departments.Finance.Entities.Sql;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Business.Departments.Finance.Repositories.Sql
{
    /// <summary>
    /// Üretilecek ürünlerin üretim talepleri tablosu için repository sınıfı
    /// </summary>
    public class ProductionRequestRepository : BaseRepository<ProductionRequestEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[FINANCE_PRODUCTIONREQUESTS]";

        /// <summary>
        /// Üretilecek ürünlerin üretim talepleri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public ProductionRequestRepository(ISqlUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Yeni üretim talebi oluşturur
        /// </summary>
        /// <param name="entity">Oluşturulacak üretim talebi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        public override async Task<int> CreateAsync(ProductionRequestEntity entity, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                      ([PRODUCTION_PRODUCTS_ID],
                                                      [AMOUNT],
                                                      [HR_DEPARTMENTS_ID],
                                                      [REFERENCE_NUMBER],
                                                      [APPROVED],
                                                      [DONE])
                                                      VALUES
                                                      (@PRODUCTION_PRODUCTS_ID,
                                                       @AMOUNT,
                                                       @HR_DEPARTMENTS_ID,
                                                       @REFERENCE_NUMBER,
                                                       @APPROVED,
                                                       @DONE);
                                                       SELECT CAST(scope_identity() AS int)",
                                              UnitOfWork.SqlConnection,
                                              UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            };

            sqlCommand.Parameters.AddWithValue("@PRODUCTION_PRODUCTS_ID", ((object)entity.ProductId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@AMOUNT", ((object)entity.Amount) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@HR_DEPARTMENTS_ID", ((object)entity.DepartmentId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@REFERENCE_NUMBER", ((object)entity.ReferenceNumber) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@APPROVED", ((object)entity.Approved) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@DONE", ((object)entity.Done) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Üretim talepleri listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<ProductionRequestEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<ProductionRequestEntity> entities = new List<ProductionRequestEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [PRODUCTION_PRODUCTS_ID],
                                                      [AMOUNT],
                                                      [HR_DEPARTMENTS_ID],
                                                      [REFERENCE_NUMBER],
                                                      [APPROVED],
                                                      [DONE]
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
                        ProductionRequestEntity inventory = new ProductionRequestEntity
                        {
                            Id = sqlDataReader.GetInt32("ID"),
                            ProductId = sqlDataReader.GetInt32("PRODUCTION_PRODUCTS_ID"),
                            Amount = sqlDataReader.GetInt32("AMOUNT"),
                            DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                            ReferenceNumber = sqlDataReader.GetInt32("REFERENCE_NUMBER"),
                            Approved = sqlDataReader.GetBoolean("APPROVED"),
                            Done = sqlDataReader.GetBoolean("DONE")
                        };

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

        /// <summary>
        /// Bir Id değerine sahip üretim talebini silindi olarak işaretler
        /// </summary>
        /// <param name="id">Silindi olarak işaretlenecek üretim talebinin Id değeri</param>
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
        /// Silindi olarak işaretlenmiş bir üretim talebi kaydının işaretini kaldırır
        /// </summary>
        /// <param name="id">Silindi işareti kaldırılacak üretim talebi kaydının Id değeri</param>
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
        /// Üretim talebi bilgisini getirir
        /// </summary>
        /// <param name="costId">Getirilecek talebin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<ProductionRequestEntity> GetAsync(int costId, CancellationTokenSource cancellationTokenSource)
        {
            ProductionRequestEntity ProductionRequestEntity = null;

            SqlCommand sqlCommand = new SqlCommand($@"SELECT
                                                      TOP 1
                                                      [ID],
                                                      [PRODUCTION_PRODUCTS_ID],
                                                      [AMOUNT],
                                                      [HR_DEPARTMENTS_ID],
                                                      [REFERENCE_NUMBER],
                                                      [APPROVED],
                                                      [DONE]
                                                      FROM {TABLE_NAME}
                                                      WHERE 
                                                      ID = @ID
                                                      AND
                                                      DELETE_DATE IS NULL",
                                                        UnitOfWork.SqlConnection,
                                                        UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", ((object)costId) ?? DBNull.Value);

            using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
            {

                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                    {
                        ProductionRequestEntity = new ProductionRequestEntity
                        {
                            Id = sqlDataReader.GetInt32("ID"),
                            ProductId = sqlDataReader.GetInt32("PRODUCTION_PRODUCTS_ID"),
                            Amount = sqlDataReader.GetInt32("AMOUNT"),
                            DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                            ReferenceNumber = sqlDataReader.GetInt32("REFERENCE_NUMBER"),
                            Approved = sqlDataReader.GetBoolean("APPROVED"),
                            Done = sqlDataReader.GetBoolean("DONE")
                        };
                    }
                }
            }

            return ProductionRequestEntity;
        }


        /// <summary>
        /// Bir üretim talebi kaydındaki bir kolon değerini değiştirir
        /// </summary>
        /// <param name="id">Değeri değiştirilecek üretim talebinin Id değeri</param>
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

        /// <summary>
        /// Üretim talebini reddeder
        /// </summary>
        /// <param name="costId">Reddedilecek üretim talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> ApproveAsync(int costId, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET APPROVED = 1, DONE = 1
                                                      WHERE ID = @ID",
                                                           UnitOfWork.SqlConnection,
                                                           UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", costId);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }

        /// <summary>
        /// Üretim talebini onaylar
        /// </summary>
        /// <param name="costId">Onaylanacak üretim talebinin Id değeri</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> RejectAsync(int costId, CancellationTokenSource cancellationTokenSource)
        {
            SqlCommand sqlCommand = new SqlCommand($@"UPDATE {TABLE_NAME}
                                                      SET APPROVED = 0, DONE = 1
                                                      WHERE ID = @ID",
                                                         UnitOfWork.SqlConnection,
                                                         UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ID", costId);

            return (int)await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
        }
    }
}
