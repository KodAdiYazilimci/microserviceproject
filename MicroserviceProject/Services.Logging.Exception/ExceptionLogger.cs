using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Elastic.Loggers;
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
        private readonly LogManager<ExceptionLogModel> _logManager;

        /// <summary>
        /// Exception loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Exception log ayarlarının çekileceği configuration</param>
        public ExceptionLogger(IConfiguration configuration)
        {
            List<ILogger<ExceptionLogModel>> loggers = new List<ILogger<ExceptionLogModel>>();

            JsonFileLogger<ExceptionLogModel> jsonFileLogger =
                new JsonFileLogger<ExceptionLogModel>(
                    new ExceptionLogFileConfiguration(configuration));

            loggers.Add(jsonFileLogger);

            TextFileLogger<ExceptionLogModel> exceptionRabbitLogger =
                new TextFileLogger<ExceptionLogModel>(
                    new ExceptionLogFileConfiguration(configuration));

            loggers.Add(exceptionRabbitLogger);

            //DefaultElasticLogger<ExceptionLogModel> elasticLogger =
            //    new DefaultElasticLogger<ExceptionLogModel>(
            //        new ExceptionLogElasticConfiguration(configuration));

            //loggers.Add(elasticLogger);

            _logManager = new LogManager<ExceptionLogModel>(loggers);
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
            await _logManager.LogAsync(model, cancellationTokenSource);
        }
    }
}
