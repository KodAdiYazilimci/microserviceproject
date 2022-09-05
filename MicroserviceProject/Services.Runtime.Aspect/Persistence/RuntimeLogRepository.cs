
using Microsoft.Extensions.Configuration;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Services.Logging.Aspect.Persistence
{
    /// <summary>
    /// Çalışma zamanı logları repository sınıfı
    /// </summary>
    public class RuntimeLogRepository : IDisposable
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
        /// Çalışma zamanı logları repository sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini verecek configuration nesnesi</param>
        public RuntimeLogRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Veritabanına bir çalışma zamanı log kaydı ekler
        /// </summary>
        /// <param name="logModel">Eklenecek logun nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task<int> InsertLogAsync(RuntimeLogModel logModel, CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;
            int result = 0;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                SqlCommand sqlCommand = new SqlCommand(@"INSERT INTO [dbo].[RUNTIME_LOGS]
                                                            (
                                                                [MACHINE_NAME],
                                                                [APPLICATION_NAME],
                                                                [LOG_TEXT],
                                                                [DATE],
                                                                [METHOD],
                                                                [PARAMETERSASJSON],
                                                                [RESULTASJSON]
                                                            )
                                                            VALUES
                                                            (
                                                                @MACHINE_NAME,
                                                                @APPLICATION_NAME,
                                                                @LOG_TEXT,
                                                                @DATE,
                                                                @METHOD,
                                                                @PARAMETERSASJSON,
                                                                @RESULTASJSON
                                                            )", sqlConnection);

                sqlCommand.Parameters.AddWithValue("@MACHINE_NAME", ((object)logModel.MachineName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@APPLICATION_NAME", ((object)logModel.ApplicationName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@LOG_TEXT", ((object)logModel.LogText) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@DATE", ((object)logModel.Date) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@METHOD", ((object)logModel.MethodName) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@PARAMETERSASJSON", ((object)logModel.ParametersAsJson) ?? DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@RESULTASJSON", ((object)logModel.ResultAsJson) ?? DBNull.Value);

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
                    Convert.ToBoolean(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Logging")
                        .GetSection("RuntimeLogging")
                        .GetSection("DataBaseConfiguration")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                        ?
                        Environment.GetEnvironmentVariable(
                            _configuration
                            .GetSection("Configuration")
                            .GetSection("Logging")
                            .GetSection("RuntimeLogging")
                            .GetSection("DataBaseConfiguration")["EnvironmentVariableName"])
                        :
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Logging")
                        .GetSection("RuntimeLogging")
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
