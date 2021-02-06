using MicroserviceProject.Services.Business.Departments.Model.Department.HR;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories
{
    public class DepartmentRepository : BaseRepository
    {
        private readonly IConfiguration _configuration;

        public DepartmentRepository(IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<DepartmentModel>> GetDepartmentsAsync(CancellationToken cancellationToken)
        {
            List<DepartmentModel> departments = new List<DepartmentModel>();

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
                            DepartmentModel department = new DepartmentModel();

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

        public async Task CreateDepartmentAsync(DepartmentModel department, CancellationToken cancellationToken)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                Exception exception = null;
                SqlTransaction sqlTransaction = null;

                try
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationToken);
                    }

                    sqlTransaction = (SqlTransaction)await sqlConnection.BeginTransactionAsync(cancellationToken);

                    SqlCommand sqlCommand = new SqlCommand(@"
                                                            INSERT INTO [DEPARTMENTS].[HR]
                                                            ([NAME])
                                                            VALUES
                                                            (@NAME)", sqlConnection, sqlTransaction);
                    sqlCommand.Transaction = sqlTransaction;

                    sqlCommand.Parameters.AddWithValue("@NAME", ((object)department.Name) ?? DBNull.Value);

                    await sqlCommand.ExecuteNonQueryAsync(cancellationToken);

                    await sqlTransaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    exception = ex;

                    await sqlTransaction?.RollbackAsync(cancellationToken);
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
        }
    }
}
