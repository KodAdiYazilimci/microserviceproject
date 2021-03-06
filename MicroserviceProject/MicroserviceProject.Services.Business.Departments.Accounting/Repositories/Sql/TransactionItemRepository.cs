using MicroserviceProject.Services.Business.Departments.Accounting.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.Accounting.Repositories.Sql
{
    /// <summary>
    /// İdari işler işlem tablosu için repository sınıfı
    /// </summary>
    public class TransactionItemRepository : BaseRepository<RollbackItemEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[ACCOUNTING_TRANSACTIONS_ITEMS]";

        /// <summary>
        /// İdari işler işlem tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public TransactionItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Yeni transaction öğesi kaydı oluşturur
        /// </summary>
        /// <param name="inventory">Oluşturulacak transaction öğesi kaydı nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        public override async Task<int> CreateAsync(RollbackItemEntity entity, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                     ([ROLLBACK_TYPE],
                                                      [NAME],
                                                      [DATASET],
                                                      [IDENTITY],
                                                      [OLD_VALUE],
                                                      [NEW_VALUE],
                                                      [IS_ROLLED_BACK])
                                                     VALUES(
                                                      @ROLLBACK_TYPE,
                                                      @NAME,
                                                      @DATASET,
                                                      @IDENTITY,
                                                      @OLD_VALUE,
                                                      @NEW_VALUE,
                                                      @IS_ROLLED_BACK);
                                                      SELECT CAST(scope_identity() AS int)",
                                              UnitOfWork.SqlConnection,
                                              UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@ROLLBACK_TYPE", ((object)entity.RollbackType) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@NAME", ((object)entity.Name) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@DATASET", ((object)entity.DataSet) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IDENTITY", ((object)entity.Identity) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@OLD_VALUE", ((object)entity.OldValue) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@NEW_VALUE", ((object)entity.NewValue) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@IS_ROLLED_BACK", ((object)entity.IsRolledback) ?? DBNull.Value);

            return (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);
        }

        /// <summary>
        /// İşlem öğelerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<RollbackItemEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<RollbackItemEntity> entities = new List<RollbackItemEntity>();

            SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [ROLLBACK_TYPE],
                                                      [NAME],
                                                      [DATASET],
                                                      [IDENTITY],
                                                      [OLD_VALUE],
                                                      [NEW_VALUE],
                                                      [IS_ROLLED_BACK]
                                                      FROM {TABLE_NAME}
                                                      WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    RollbackItemEntity inventory = new RollbackItemEntity();

                    inventory.Id = sqlDataReader.GetInt32("ID");
                    inventory.RollbackType = sqlDataReader.GetInt32("ROLLBACK_TYPE");
                    inventory.Name = sqlDataReader.GetString("NAME");
                    inventory.DataSet = sqlDataReader.GetString("DATASET");
                    inventory.Identity = sqlDataReader.GetString("IDENTITY");
                    inventory.OldValue = sqlDataReader.GetString("OLD_VALUE");
                    inventory.NewValue = sqlDataReader.GetString("NEW_VALUE");
                    inventory.IsRolledback = sqlDataReader.GetBoolean("IS_ROLLED_BACK");

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

                    disposed = true;
                }
            }
        }
    }
}
