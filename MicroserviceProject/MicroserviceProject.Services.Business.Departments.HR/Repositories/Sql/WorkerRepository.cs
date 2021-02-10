using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Util.UnitOfWork;

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
                                                     [ID]
                                                     [NAME],
                                                     [FROMDATE],
                                                     [TODATE],
                                                     [DEPARTMENTID],
                                                     [PERSONID],
                                                     [TITLEID]
                                                     FROM [WORKERS]
                                                     WHERE DELETEDATE IS NULL",
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
                    worker.Name = sqlDataReader.GetString("NAME");
                    worker.FromDate = sqlDataReader.GetDateTime("FROMDATE");
                    worker.ToDate = sqlDataReader.GetDateTime("TODATE");
                    worker.DepartmentId = sqlDataReader.GetInt32("DEPARTMENTID");
                    worker.PersonId = sqlDataReader.GetInt32("PERSONID");
                    worker.TitleId = sqlDataReader.GetInt32("TITLEID");

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
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [WORKERS]
                                                    ([FROMDATE],
                                                    [TODATE],
                                                    [DEPARTMENTID],
                                                    [PERSONID],
                                                    [TITLEID])
                                                    VALUES
                                                    (@FROMDATE,
                                                     @TODATE,
                                                     @DEPARTMENTID,
                                                     @PERSONID,
                                                     @TITLEID);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@FROMDATE", ((object)worker.FromDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TODATE", ((object)worker.ToDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@DEPARTMENTID", ((object)worker.DepartmentId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@PERSONID", ((object)worker.PersonId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TITLEID", ((object)worker.TitleId) ?? DBNull.Value);

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
