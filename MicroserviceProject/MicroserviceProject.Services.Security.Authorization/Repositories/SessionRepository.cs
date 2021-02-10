
using MicroserviceProject.Infrastructure.Security.Model;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Persistence.Sql.Repositories
{
    /// <summary>
    /// Oturum repository sınıfı
    /// </summary>
    public class SessionRepository
    {
        /// <summary>
        /// Veritabanı bağlantı cümlesi
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Oturum repository sınıfı
        /// </summary>
        /// <param name="connectionString">Veritabanı bağlantı cümlesi</param>
        public SessionRepository(string connectionString) //: base(connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Oturum bilgisini kaydeder ve oturumun kimliğini verir
        /// </summary>
        /// <param name="userId">Sorgulanacak kullanıcının Id değeri</param>
        /// <returns></returns>
        public async Task<int> InsertSessionAsync(int userId, string token, DateTime validTo, string ipAddress, string userAgent, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            int generatedSessionId = 0;
            Exception exception = null;

            DateTime currentDate = DateTime.Now;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

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
                    await sqlConnection.OpenAsync(cancellationToken);
                }

                generatedSessionId = await sqlCommand.ExecuteNonQueryAsync(cancellationToken);
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
        public async Task<Session> GetValidSessionAsync(string token, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Session session = null;

            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                SqlCommand sqlCommand =
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
                                      ISVALID = 1", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@TOKEN", token);

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

                        session = new Session
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
    }
}
