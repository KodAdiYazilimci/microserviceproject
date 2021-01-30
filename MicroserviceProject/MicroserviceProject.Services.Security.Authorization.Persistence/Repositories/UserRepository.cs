using MicroserviceProject.Model.Security;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Persistence.Repositories
{
    /// <summary>
    /// Kullanıcı repository sınıfı
    /// </summary>
    public class UserRepository
    {
        /// <summary>
        /// Veritabanı bağlantı cümlesi
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Kullanıcı repository sınıfı
        /// </summary>
        /// <param name="connectionString">Veritabanı bağlantı cümlesi</param>
        public UserRepository(string connectionString) //: base(connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Kullanıcıların listesini verir
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            List<User> users = new List<User>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand(@"
                                    SELECT * FROM USERS 
                                    WHERE 
                                    DELETEDATE IS NULL", sqlConnection);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        User user = new User
                        {
                            Id = Convert.ToInt32(sqlDataReader["ID"])
                        };
                        user.Name = sqlDataReader["NAME"].ToString();
                        user.IsAdmin = Convert.ToBoolean(sqlDataReader["ISADMIN"]);

                        users.Add(user);
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

            return users;
        }

        /// <summary>
        /// Kullanıcıyı verir
        /// </summary>
        /// <param name="userId">Getirilecek kullanıcının Id değeri</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(int userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            User user = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE ID = @id AND DELETEDATE IS NULL", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@id", userId);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        user = new User
                        {
                            Id = Convert.ToInt32(sqlDataReader["ID"])
                        };
                        user.Name = sqlDataReader["NAME"].ToString();
                        user.Email = sqlDataReader["EMAIL"].ToString();
                        user.IsAdmin = Convert.ToBoolean(sqlDataReader["ISADMIN"]);
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

            return user;
        }

        /// <summary>
        /// Kullanıcıyı verir
        /// </summary>
        /// <param name="email">Kullanıcının e-posta adresi</param>
        /// <param name="password">Kullanıcının parolası</param>
        /// <returns></returns>
        public async Task<User> GetUserAsync(string email, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            User user = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE EMAIL = @EMAIL AND PASSWORD=@PASSWORD AND DELETEDATE IS NULL", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EMAIL", email);
                sqlCommand.Parameters.AddWithValue("@PASSWORD", password);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                if (sqlDataReader.HasRows)
                {
                    while (await sqlDataReader.ReadAsync(cancellationToken))
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        user = new User
                        {
                            Id = Convert.ToInt32(sqlDataReader["ID"])
                        };
                        user.Name = sqlDataReader["NAME"].ToString();
                        user.Password = sqlDataReader["PASSWORD"].ToString();
                        user.Email = sqlDataReader["EMAIL"].ToString();
                        user.IsAdmin = Convert.ToBoolean(sqlDataReader["ISADMIN"]);
                    }
                }
                else
                {
                    user = null;
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

            return user;
        }

        /// <summary>
        /// Kullanıcı kaydı oluşturur
        /// </summary>
        /// <param name="email">Kullanıcının e-posta adresi</param>
        /// <param name="password">Kullanıcının parolası</param>
        public async Task RegisterAsync(string email, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"
                                                        INSERT INTO [dbo].[USERS]
                                                        (
                                                            [CREATEDATE],
                                                            [UPDATEDATE],
                                                            [EMAIL],
                                                            [NAME],
                                                            [PASSWORD],
                                                            [ISADMIN]
                                                        )
                                                        VALUES
                                                        (
                                                            GETDATE(),
                                                            GETDATE(),
                                                            @EMAIL,
                                                            @NAME,
                                                            @PASSWORD,
                                                            0
                                                        )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@NAME", email);
                sqlCommand.Parameters.AddWithValue("@EMAIL", email);
                sqlCommand.Parameters.AddWithValue("@PASSWORD", password);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
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

        }

        /// <summary>
        /// Kullanıcı varlığını denetler
        /// </summary>
        /// <param name="email">Kullanıcının kontrol edileceği e-posta adresi</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAsync(string email, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            bool isExists = false;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE EMAIL = @EMAIL AND DELETEDATE IS NULL", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@EMAIL", email);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationToken);

                if (sqlDataReader.HasRows)
                {
                    isExists = true;
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

            return isExists;
        }
    }
}
