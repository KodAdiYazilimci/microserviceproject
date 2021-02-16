using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.UnitOfWork;
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
    public class WorkerRepository : BaseRepository<WorkerEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<WorkerEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<WorkerEntity> workers = new List<WorkerEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT 
                                                     [ID],
                                                     [FROM_DATE],
                                                     [TO_DATE],
                                                     [HR_DEPARTMENTS_ID],
                                                     [HR_PEOPLE_ID],
                                                     [HR_TITLES_ID]
                                                     FROM [HR_WORKERS]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    WorkerEntity worker = new WorkerEntity();

                    worker.Id = sqlDataReader.GetInt32("ID");
                    worker.FromDate = sqlDataReader.GetDateTime("FROM_DATE");
                    worker.ToDate =
                        sqlDataReader.IsDBNull(sqlDataReader.GetOrdinal("TO_DATE"))
                        ?
                        null
                        :
                        sqlDataReader.GetDateTime("TO_DATE");
                    worker.DepartmentId = sqlDataReader.GetInt32("HR_DEPARTMENTS_ID");
                    worker.PersonId = sqlDataReader.GetInt32("HR_PEOPLE_ID");
                    worker.TitleId = sqlDataReader.GetInt32("HR_TITLES_ID");

                    workers.Add(worker);
                }
            }

            return workers;
        }

        /// <summary>
        /// Yeni çalışan oluşturur
        /// </summary>
        /// <param name="worker">Oluşturulacak çalışan nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(WorkerEntity worker, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [HR_WORKERS]
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
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)worker.FromDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)worker.ToDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@HR_DEPARTMENTS_ID", ((object)worker.DepartmentId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@HR_PEOPLE_ID", ((object)worker.PersonId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@HR_TITLES_ID", ((object)worker.TitleId) ?? DBNull.Value);

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
