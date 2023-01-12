using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.File.Loggers;
using Infrastructure.Logging.Managers;

using Microsoft.Extensions.Configuration;

using Services.Logging.Exception.Configuration;

namespace Services.Logging.Exception
{
    /// <summary>
    /// Exception loglarını yazan sınıf
    /// </summary>
    public class ExceptionLogger : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log yazma işlemlerini yürütecek yönetici
        /// </summary>
        private readonly BulkLogManager<ExceptionLogModel> _logManager;

        private List<ExceptionLogModel> exceptionLogModels = new List<ExceptionLogModel>();

        /// <summary>
        /// Exception loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Exception log ayarlarının çekileceği configuration</param>
        public ExceptionLogger(IConfiguration configuration)
        {
            List<IBulkLogger<ExceptionLogModel>> loggers = new List<IBulkLogger<ExceptionLogModel>>();

            BulkJsonFileLogger<ExceptionLogModel> jsonFileLogger =
                new BulkJsonFileLogger<ExceptionLogModel>(
                    new ExceptionLogFileConfiguration(configuration));

            BulkTextFileLogger<ExceptionLogModel> exceptionRabbitLogger =
                new BulkTextFileLogger<ExceptionLogModel>(
                    new ExceptionLogFileConfiguration(configuration));

            loggers.Add(exceptionRabbitLogger);

            loggers.Add(jsonFileLogger);

            _logManager = new BulkLogManager<ExceptionLogModel>(loggers);
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
                    if (_logManager != null)
                        _logManager.Dispose();

                    if (exceptionLogModels != null)
                    {
                        exceptionLogModels.Clear();
                        exceptionLogModels = null;
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak Exception logun nesnesi</param>
        public async Task LogAsync(ExceptionLogModel model, CancellationTokenSource cancellationTokenSource)
        {
            if (exceptionLogModels.Count > 100)
            {
                ExceptionLogModel[] tempExceptionLogs = new ExceptionLogModel[exceptionLogModels.Count];
                exceptionLogModels.CopyTo(tempExceptionLogs);
                exceptionLogModels.Clear();

                await _logManager.LogAsync(tempExceptionLogs.ToList(), cancellationTokenSource);
            }
            else
            {
                exceptionLogModels.Add(model);
            }
        }
    }
}
