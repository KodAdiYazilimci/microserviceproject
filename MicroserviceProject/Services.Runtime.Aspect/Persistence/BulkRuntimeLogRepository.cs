
using Infrastructure.Logging.Abstraction;

using Microsoft.Extensions.Configuration;

using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Services.Logging.Aspect.Persistence
{
    /// <summary>
    /// Çalışma zamanı logları repository sınıfı
    /// </summary>
    public class BulkRuntimeLogRepository : IBulkLogger<RuntimeLogModel>, IDisposable
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
        public BulkRuntimeLogRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        /// <summary>
        /// Veritabanına bir çalışma zamanı log kaydı ekler
        /// </summary>
        /// <param name="logModels">Eklenecek logun nesnesi</param>
        /// <param name="cancellationTokenSource">İptal tokenı</param>
        /// <returns></returns>
        public async Task LogAsync(List<RuntimeLogModel> logModels, CancellationTokenSource cancellationTokenSource)
        {
            Exception exception = null;

            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("MACHINE_NAME", typeof(string));
                dataTable.Columns.Add("APPLICATION_NAME", typeof(string));
                dataTable.Columns.Add("LOG_TEXT", typeof(string));
                dataTable.Columns.Add("DATE", typeof(DateTime));
                dataTable.Columns.Add("METHOD", typeof(string));
                dataTable.Columns.Add("PARAMETERSASJSON", typeof(string));
                dataTable.Columns.Add("RESULTASJSON", typeof(string));

                foreach (var logItem in logModels.Where(x => x != null))
                {
                    var row = dataTable.NewRow();
                    row["MACHINE_NAME"] = logItem.MachineName;
                    row["APPLICATION_NAME"] = logItem.ApplicationName;
                    row["LOG_TEXT"] = logItem.LogText ?? string.Empty;
                    row["DATE"] = logItem.Date;
                    row["METHOD"] = logItem.MethodName;
                    row["PARAMETERSASJSON"] = logItem.ParametersAsJson;
                    row["RESULTASJSON"] = logItem.ResultAsJson;

                    dataTable.Rows.Add(row);
                }

                if (sqlConnection.State != ConnectionState.Open)
                {
                    await sqlConnection.OpenAsync(cancellationTokenSource.Token);
                }

                using (var bulk = new SqlBulkCopy(sqlConnection))
                {
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("MACHINE_NAME", "MACHINE_NAME"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("APPLICATION_NAME", "APPLICATION_NAME"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("LOG_TEXT", "LOG_TEXT"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("DATE", "DATE"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("METHOD", "METHOD"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("PARAMETERSASJSON", "PARAMETERSASJSON"));
                    bulk.ColumnMappings.Add(new SqlBulkCopyColumnMapping("RESULTASJSON", "RESULTASJSON"));

                    bulk.DestinationTableName = "RUNTIME_LOGS";
                    await bulk.WriteToServerAsync(dataTable, cancellationTokenSource.Token);
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
