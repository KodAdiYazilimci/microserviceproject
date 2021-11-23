
using Microsoft.Extensions.Configuration;

using Services.Logging.RequestResponse.Configuration;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Logging.RequestResponse.Persistence
{
    /// <summary>
    /// Request-Response logları repository sınıfı
    /// </summary>
    public class RequestResponseLogRepository : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Veritabanı bağlantı cümlesini verecek configuration nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Request-Response logları repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini verecek configuration nesnesi</param>
        public RequestResponseLogRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Veritabanına bir request-response log kaydı ekler
        /// </summary>
        /// <param name="requestResponseLogModel">Eklenecek logun nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> InsertLogAsync(RequestResponseLogModel requestResponseLogModel, CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;
            int result = 0;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[REQUEST_RESPONSE_LOGS]
                                                            (
                                                                [MACHINE_NAME],
                                                                [APPLICATION_NAME],
                                                                [LOG_TEXT],
                                                                [DATE],
                                                                [CONTENT],
                                                                [REQUEST_CONTENT_LENGTH],
                                                                [HOST],
                                                                [IPADDRESS],
                                                                [METHOD],
                                                                [PROTOCOL],
                                                                [RESPONSE_CONTENT_LENGTH],
                                                                [RESPONSE_CONTENT_TYPE],
                                                                [RESPONSE_TIME],
                                                                [STATUS_CODE],
                                                                [URL]
                                                            )
                                                            VALUES
                                                            (
                                                                @MACHINE_NAME,
                                                                @APPLICATION_NAME,
                                                                @LOG_TEXT,
                                                                @DATE,
                                                                @CONTENT,
                                                                @REQUEST_CONTENT_LENGTH,
                                                                @HOST,
                                                                @IPADDRESS,
                                                                @METHOD,
                                                                @PROTOCOL,
                                                                @RESPONSE_CONTENT_LENGTH,
                                                                @RESPONSE_CONTENT_TYPE,
                                                                @RESPONSE_TIME,
                                                                @STATUS_CODE,
                                                                @URL
                                                            )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MACHINE_NAME", ((object)requestResponseLogModel.MachineName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@APPLICATION_NAME", ((object)requestResponseLogModel.ApplicationName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@LOG_TEXT", ((object)requestResponseLogModel.LogText) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DATE", ((object)requestResponseLogModel.Date) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@CONTENT", ((object)requestResponseLogModel.Content) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@REQUEST_CONTENT_LENGTH", ((object)requestResponseLogModel.RequestContentLength) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@HOST", ((object)requestResponseLogModel.Host) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@IPADDRESS", ((object)requestResponseLogModel.IpAddress) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@METHOD", ((object)requestResponseLogModel.Method) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@PROTOCOL", ((object)requestResponseLogModel.Protocol) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@RESPONSE_CONTENT_LENGTH", ((object)requestResponseLogModel.ResponseContentLength) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@RESPONSE_CONTENT_TYPE", ((object)requestResponseLogModel.ResponseContentType) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@RESPONSE_TIME", ((object)requestResponseLogModel.ResponseTime) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@STATUS_CODE", ((object)requestResponseLogModel.StatusCode) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@URL", ((object)requestResponseLogModel.Url) ?? DBNull.Value);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                }

                result = await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
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

            return result;
        }

        /// <summary>
        /// Request-response loglarının yazılacağı veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <returns></returns>
        private string ConnectionString
        {
            get
            {
                string connectionString =
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RequestResponseLogging")
                    .GetSection("DataBaseConfiguration")["DataSource"];

                return connectionString;
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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

                }

                disposed = true;
            }
        }
    }
}
