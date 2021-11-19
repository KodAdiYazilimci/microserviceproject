
using Infrastructure.Security.Model;

using Microsoft.Extensions.Configuration;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Infrastructure.Authorization.Persistence.Sql.Repositories
{
    /// <summary>
    /// Oturum repository sınıfı
    /// </summary>
    public class SessionRepository : BaseRepository, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Oturum repository sınıfı
        /// </summary>
        /// <param name="connectionString">Veritabanı bağlantı cümlesini getirecek configuration nesnesi</param>
        public SessionRepository(IConfiguration configuration) : base(configuration)
        {

        }

        /// <summary>
        /// Oturum bilgisini kaydeder ve oturumun kimliğini verir
        /// </summary>
        /// <param name="userId">Sorgulanacak kullanıcının Id değeri</param>
        /// <returns></returns>
        public async Task<int> InsertSessionAsync(int userId, string token, DateTime validTo, string ipAddress, string userAgent, CancellationTokenSource cancellationTokenSource)
        {
            int generatedSessionId = 0;
            Exception exception = null;

            DateTime currentDate = DateTime.Now;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                SqlCommand sqlCommand =
                    new SqlCommand($@"
                                    INSERT INTO SESSIONS
                                    (
                                        CREATEDATE,
                                        UPDATEDATE,
                                        IPADDRESS,
                                        ISVALID,
                                        TOKEN,
                                        USERAGENT,
                                        USERID,
                                        VALIDTO
                                    )
                                    VALUES
                                    (
                                        @CREATEDATE,
                                        @UPDATEDATE,
                                        @IPADDRESS,
                                        @ISVALID,
                                        @TOKEN,
                                        @USERAGENT,
                                        @USERID,
                                        @VALIDTO
                                    )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@CREATEDATE", currentDate);
                sqlCommand.Parameters.AddWithValue("@UPDATEDATE", currentDate);
                sqlCommand.Parameters.AddWithValue("@IPADDRESS", ipAddress);
                sqlCommand.Parameters.AddWithValue("@ISVALID", true);
                sqlCommand.Parameters.AddWithValue("@TOKEN", token);
                sqlCommand.Parameters.AddWithValue("@USERAGENT", userAgent);
                sqlCommand.Parameters.AddWithValue("@USERID", userId);
                sqlCommand.Parameters.AddWithValue("@VALIDTO", validTo);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                }

                generatedSessionId = await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
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

            return generatedSessionId;
        }

        /// <summary>
        /// Token bilgisine göre halen devam eden oturum bilgisini verir, zamanaşımına uğramışsa null döner.
        /// </summary>
        /// <param name="token">Oturumun token anahtarı</param>
        /// <returns></returns>
        public async Task<AuthenticationSession> GetValidSessionAsync(string token, CancellationTokenSource cancellationTokenSource)
        {
            AuthenticationSession session = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(AuthorizationConnectionString);

            try
            {
                using (SqlCommand sqlCommand =
                       new SqlCommand(@"
                                    SELECT [ID]
                                          ,[IPADDRESS]
                                          ,[TOKEN]
                                          ,[USERAGENT]
                                          ,[USERID]
                                          ,[VALIDTO]
                                      FROM [dbo].[SESSIONS]
                                      WHERE 
                                      TOKEN = @TOKEN 
                                      AND 
                                      VALIDTO > GETDATE()
                                      AND
                                      DELETE_DATE IS NULL
                                      AND
                                      ISVALID = 1", sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@TOKEN", token);

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
                                session = new AuthenticationSession
                                {
                                    Id = Convert.ToInt32(sqlDataReader["ID"])
                                };

                                session.Token = sqlDataReader["TOKEN"].ToString();
                                session.UserAgent = sqlDataReader["USERAGENT"].ToString();
                                session.UserId = Convert.ToInt32(sqlDataReader["USERID"]);
                                session.ValidTo = Convert.ToDateTime(sqlDataReader["VALIDTO"]);
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

            return session;
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
