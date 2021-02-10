﻿using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
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
    /// Çalışan ilişkileri tablosu için repository sınıfı
    /// </summary>
    public class WorkerRelationRepository : BaseRepository<WorkerRelationEntity>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışan ilişkileri tablosu için repository sınıfı
        /// </summary>
        /// <param name="unitOfWork">Veritabanı işlemlerini kapsayan iş birimi nesnesi</param>
        public WorkerRelationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        /// <summary>
        /// Çalışan ilişkilerinin listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<List<WorkerRelationEntity>> GetListAsync(CancellationToken cancellationToken)
        {
            List<WorkerRelationEntity> workerRelations = new List<WorkerRelationEntity>();

            SqlCommand sqlCommand = new SqlCommand(@"SELECT [ID]
                                                     [WORKERS_ID],
                                                     [HR_WORKERS_MANAGER_ID],
                                                     [FROM_DATE],
                                                     [TO_DATE]
                                                     FROM [HR_WORKERRELATIONS]
                                                     WHERE DELETE_DATE IS NULL",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

            if (sqlDataReader.HasRows)
            {
                while (await sqlDataReader.ReadAsync(cancellationToken))
                {
                    WorkerRelationEntity workerRelation = new WorkerRelationEntity();

                    workerRelation.Id = sqlDataReader.GetInt32("ID");
                    workerRelation.WorkerId = sqlDataReader.GetInt32("HR_WORKERS_ID");
                    workerRelation.ManagerId = sqlDataReader.GetInt32("HR_WORKERS_MANAGER_ID");
                    workerRelation.FromDate = sqlDataReader.GetDateTime("FROM_DATE");
                    workerRelation.ToDate = sqlDataReader.GetDateTime("TO_DATE");

                    workerRelations.Add(workerRelation);
                }
            }

            return workerRelations;
        }

        /// <summary>
        /// Yeni çalışan ilişkisi oluşturur
        /// </summary>
        /// <param name="workerRelation">Oluşturulacak çalışan ilişkisi nesnesi</param><
        /// <param name="unitOfWork">Oluşturma esnasında kullanılacak transaction nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public override async Task<int> CreateAsync(WorkerRelationEntity workerRelation, CancellationToken cancellationToken)
        {
            SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [HR_WORKERRELATIONS]
                                                     ([WORKERS_ID]
                                                     [HR_WORKERS_MANAGER_ID]
                                                     [FROM_DATE]
                                                     [TO_DATE])
                                                     VALUES
                                                     (@HR_WORKERS_ID,
                                                     @HR_WORKERS_MANAGER_ID,
                                                     @FROM_DATE,
                                                     @TO_DATE);
                                                     SELECT CAST(scope_identity() AS int)",
                                                     UnitOfWork.SqlConnection,
                                                     UnitOfWork.SqlTransaction);

            sqlCommand.Transaction = UnitOfWork.SqlTransaction;

            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_ID", ((object)workerRelation.WorkerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@HR_WORKERS_MANAGER_ID", ((object)workerRelation.ManagerId) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@FROM_DATE", ((object)workerRelation.FromDate) ?? DBNull.Value);
            sqlCommand.Parameters.AddWithValue("@TO_DATE", ((object)workerRelation.ToDate) ?? DBNull.Value);

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
