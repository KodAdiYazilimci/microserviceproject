
using Infrastructure.Security.Model;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Gateway.Public.Repositories
{
    /// <summary>
    /// Kullanıcı repository sınıfı
    /// </summary>
    public class UserRepository : BaseRepository, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Kullanıcı repository sınıfı
        /// </summary>
        /// <param name="connectionString">Veritabanı bağlantı cümlesini getirecek configuration nesnesi</param>
        public UserRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Kullanıcıların listesini verir
        /// </summary>
        /// <returns></returns>
        public async Task<List<AuthenticatedUser>> GetUsersAsync(CancellationTokenSource cancellationTokenSource)
        {
            List<AuthenticatedUser> users = new List<AuthenticatedUser>();

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand =
                        new SqlCommand(@"
                                    SELECT * FROM USERS 
                                    WHERE 
                                    DELETE_DATE IS NULL", sqlConnection))
                {
                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                AuthenticatedUser user = new AuthenticatedUser
                                {
                                    Id = Convert.ToInt32(sqlDataReader["ID"])
                                };
                                user.Name = sqlDataReader["NAME"].ToString();
                                user.IsAdmin = Convert.ToBoolean(sqlDataReader["ISADMIN"]);

                                users.Add(user);
                            }
                        }
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
        public async Task<AuthenticatedUser> GetUserAsync(int userId, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser user = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE ID = @id AND DELETE_DATE IS NULL", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@id", userId);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                user = new AuthenticatedUser
                                {
                                    Id = Convert.ToInt32(sqlDataReader["ID"])
                                };
                                user.Name = sqlDataReader["NAME"].ToString();
                                user.Email = sqlDataReader["EMAIL"].ToString();
                                user.IsAdmin = Convert.ToBoolean(sqlDataReader["ISADMIN"]);
                            }
                        }
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
        public async Task<AuthenticatedUser> GetUserAsync(string email, string password, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticatedUser user = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE EMAIL = @EMAIL AND PASSWORD=@PASSWORD AND DELETE_DATE IS NULL", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EMAIL", email);
                    sqlCommand.Parameters.AddWithValue("@PASSWORD", password);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (await sqlDataReader.ReadAsync(cancellationTokenSource.Token))
                            {
                                user = new AuthenticatedUser
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
        public async Task RegisterAsync(string email, string password, CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(@"
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
                                                        )", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@NAME", email);
                    sqlCommand.Parameters.AddWithValue("@EMAIL", email);
                    sqlCommand.Parameters.AddWithValue("@PASSWORD", password);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
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

        }

        /// <summary>
        /// Kullanıcı varlığını denetler
        /// </summary>
        /// <param name="email">Kullanıcının kontrol edileceği e-posta adresi</param>
        /// <returns></returns>
        public async Task<bool> CheckUserAsync(string email, CancellationTokenSource cancellationTokenSource)
        {
            bool isExists = false;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand = new SqlCommand(@"SELECT TOP 1 * FROM USERS WHERE EMAIL = @EMAIL AND DELETE_DATE IS NULL", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EMAIL", email);

                    if (sqlConnection.State != ConnectionState.Open)
                    {
                        await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                    }

                    using (SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync(cancellationTokenSource.Token))
                    {
                        if (sqlDataReader.HasRows)
                        {
                            isExists = true;
                        }
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

            return isExists;
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
                    disposed = true;
                }
            }
        }
    }
}
