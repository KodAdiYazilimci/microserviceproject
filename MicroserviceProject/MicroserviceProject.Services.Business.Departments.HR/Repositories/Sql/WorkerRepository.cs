using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Transaction;
using MicroserviceProject.Services.UnitOfWork;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Çalışan tablosu için repository sınıfı
    /// </summary>
    public class WorkerRepository : BaseRepository<WorkerEntity>, IRollbackableDataAsync<int>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Repositorynin ait olduğu tablonun adı
        /// </summary>
        public const string TABLE_NAME = "[dbo].[HR_WORKERS]";

        /// <summary>
        /// Çalışan tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public WorkerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Ünvanların listesini verir
        /// </summary>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<WorkerEntity>> GetListAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<WorkerEntity> workers = new List<WorkerEntity>();

            using (SqlCommand sqlCommand = new SqlCommand($@"SELECT 
                                                      [ID],
                                                      [FROM_DATE],
                                                      [TO_DATE],
                                                      [HR_DEPARTMENTS_ID],
                                                      [HR_PEOPLE_ID],
                                                      [HR_TITLES_ID]
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
                            WorkerEntity worker = new WorkerEntity
                            {
                                Id = sqlDataReader.GetInt32("ID"),
                                FromDate = sqlDataReader.GetDateTime("FROM_DATE"),
                                ToDate =
                                sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TO_DATE"))
                                ?
                                null
                                :
                                sqlDataReader.GetDateTime("TO_DATE"),
                                DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID"),
                                PersonId = sqlDataReader.GetInt32("HR_PEOPLE_ID"),
                                TitleId = sqlDataReader.GetInt32("HR_TITLES_ID")
                            };

                            workers.Add(worker);
                        }
                    }

                    return workers;
                }
            }
        }

        /// <summary>
        /// Yeni çalışan oluşturur
        /// </summary>
        /// <param name="worker">Oluşturulacak çalışan nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(WorkerEntity worker, CancellationTokenSource cancellationTokenSource)
        {
            using (SqlCommand sqlCommand = new SqlCommand($@"INSERT INTO {TABLE_NAME}
                                                     ([FROM_DATE],
                                                     [TO_DATE],
                                                     [HR_DEPARTMENTS_ID],
                                                     [HR_PEOPLE_ID],
                                                     [HR_TITLES_ID])
                                                     VALUES
                                                     (@FROM_DATE,
                                                     @TO_DATE,
                                                     @HR_DEPARTMENTS_ID,
                                                     @HR_PEOPLE_ID,
                                                     @HR_TITLES_ID);
                                                     SELECT CAST(scope_identity() AS int)",
                                                       UnitOfWork.SqlConnection,
                                                       UnitOfWork.SqlTransaction)
            {
                Transaction = UnitOfWork.SqlTransaction
            })
            {
                sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)worker.FromDate) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)worker.ToDate) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@HR_DEPARTMENTS_ID", ((object)worker.DepartmentId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@HR_PEOPLE_ID", ((object)worker.PersonId) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@HR_TITLES_ID", ((object)worker.TitleId) ?? DBNull.Value);

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
