
using Microsoft.Extensions.Configuration;

using Services.Logging.Exception.Configuration;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Services.Logging.Exception.Persistence
{
    /// <summary>
    /// Exception logları repository sınıfı
    /// </summary>
    public class ExceptionLogRepository : IDisposable
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
        /// Exception logları repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini verecek configuration nesnesi</param>
        public ExceptionLogRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Veritabanına bir Exception log kaydı ekler
        /// </summary>
        /// <param name="exceptionLogModel">Eklenecek logun nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> InsertLogAsync(ExceptionLogModel exceptionLogModel, CancellationTokenSource cancellationTokenSource)
        {
            System.Exception exception = null;
            int result = 0;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[EXCEPTION_LOGS]
                                                            (
                                                                [MACHINE_NAME],
                                                                [APPLICATION_NAME],
                                                                [LOG_TEXT],
                                                                [DATE],
                                                                [CONTENT],
                                                                [EXCEPTION],
                                                                [INNER_EXCEPTION]
                                                            )
                                                            VALUES
                                                            (
                                                                @MACHINE_NAME,
                                                                @APPLICATION_NAME,
                                                                @LOG_TEXT,
                                                                @DATE,
                                                                @CONTENT,
                                                                @EXCEPTION,
                                                                @INNER_EXCEPTION
                                                            )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MACHINE_NAME", ((object)exceptionLogModel.MachineName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@APPLICATION_NAME", ((object)exceptionLogModel.ApplicationName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@LOG_TEXT", ((object)exceptionLogModel.LogText) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DATE", ((object)exceptionLogModel.Date) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@CONTENT", string.Empty);
                sqlCommand.Parameters.AddWithValue("@EXCEPTION", ((object)exceptionLogModel.ExceptionMessage) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@INNER_EXCEPTION", ((object)exceptionLogModel.InnerExceptionMessage) ?? DBNull.Value);

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                }

                result = await sqlCommand.ExecuteNonQueryAsync(cancellationTokenSource.Token);
            }
            catch (System.Exception ex)
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
        /// Exception loglarının yazılacağı veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <returns></returns>
        private string ConnectionString
        {
            get
            {
                string connectionString =
                    Convert.ToBoolean(
                        _configuration
                        .GetSection("Persistence")
                        .GetSection("Databases")
                        .GetSection("Microservice_Logs_DB")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(
                        _configuration
                        .GetSection("Persistence")
                        .GetSection("Databases")
                        .GetSection("Microservice_Logs_DB")["EnvironmentVariableName"])
                    :
                    _configuration
                    .GetSection("Persistence")
                    .GetSection("Databases")
                    .GetSection("Microservice_Logs_DB")["ConnectionString"];

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
