using MicroserviceProject.Services.Business.Departments.HR.Entities.Sql;
using MicroserviceProject.Services.Business.Departments.HR.Util.UnitOfWork;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Departman tablosu için repository sınıfı
    /// </summary>
    public class DepartmentRepository : BaseRepository
    {
        /// <summary>
        /// Repository yapılandırmaları için configuration nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Departman tablosu için repository sınıfı
        /// </summary>
        /// <param name="configuration">Repository yapılandırmaları için configuration nesnesi</param>
        public DepartmentRepository(IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Departmanların listesini verir
        /// </summary>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<List<DepartmentEntity>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            List<DepartmentEntity> departments = new List<DepartmentEntity>();

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                Exception exception = null;

                try
                {
                    SqlCommand sqlCommand = new SqlCommand(@"
                                                            SELECT 
                                                            [ID],
                                                            [NAME],
                                                            [DELETEDATE]
                                                            FROM [DEPARTMENTS].[HR]
                                                            WHERE DELETEDATE IS NULL", sqlConnection);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationToken);
                    }

                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                    if (sqlDataReader.HasRows)
                    {
                        while (await sqlDataReader.ReadAsync(cancellationToken))
                        {
                            DepartmentEntity department = new DepartmentEntity();

                            department.Id = sqlDataReader.GetInt32("ID");
                            department.Name = sqlDataReader.GetString("NAME");

                            departments.Add(department);
                        }
                    }
                }
                catch (Exception ex)
                {
                    exception = ex;
                }
                finally
                {
                    if (sqlConnection.State != ConnectionState.Closed)
                    {
                        await sqlConnection.CloseAsync();
                    }
                }

                if (exception != null)
                {
                    throw exception;
                }
            };

            return departments;
        }

        /// <summary>
        /// Yeni departman oluşturur
        /// </summary>
        /// <param name="department">Oluşturulacak departman nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> CreateDepartmentAsync(DepartmentEntity department, CancellationToken cancellationToken)
        {
            int generatedId = 0;

            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            using (UnitOfWork unitOfWork = new UnitOfWork(sqlConnection))
            {
                SqlCommand sqlCommand = new SqlCommand(@"
                                                         INSERT INTO [DEPARTMENTS].[HR]
                                                         ([NAME])
                                                         VALUES
                                                         (@NAME);
                                                         SELECT CAST(scope_identity() AS int)",
                                                         sqlConnection,
                                                         unitOfWork.SqlTransaction);

                sqlCommand.Transaction = unitOfWork.SqlTransaction;

                sqlCommand.Parameters.AddWithValue("@NAME", ((object)department.Name) ?? DBNull.Value);

                generatedId = (int)await sqlCommand.ExecuteScalarAsync(cancellationToken);

                await unitOfWork.SaveAsync(cancellationToken);
            };

            return generatedId;
        }
    }
}
