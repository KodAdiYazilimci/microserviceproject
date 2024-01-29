using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Managers;
using Infrastructure.Logging.RabbitMq.Producers;

using Microsoft.Extensions.Configuration;

using Services.Logging.Aspect.Configuration;
using Services.Logging.Aspect.Persistence;

namespace Services.Logging.Aspect
{
    /// <summary>
    /// Çalışma zamanı loglarını yazan sınıf
    /// </summary>
    public class RuntimeLogger : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log yazma işlemlerini yürütecek yönetici
        /// </summary>
        private readonly LogManager<RuntimeLogModel> _logManager;

        /// <summary>
        /// Çalışma zamanı loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Çalışma zamanı log ayarlarının çekileceği configuration</param>
        public RuntimeLogger(IConfiguration configuration)
        {
            List<ILogger<RuntimeLogModel>> loggers = new List<ILogger<RuntimeLogModel>>();

            //BulkJsonFileLogger<RuntimeLogModel> jsonFileLogger =
            //    new BulkJsonFileLogger<RuntimeLogModel>(
            //        new RuntimeLogFileConfiguration(configuration));

            DefaultLogProducer<RuntimeLogModel> requestResponseRabbitLogger =
                new DefaultLogProducer<RuntimeLogModel>(
                    new RuntimeLogRabbitConfiguration(configuration));

            RuntimeLogRepository runtimeLogRepository = new RuntimeLogRepository(configuration);

            loggers.Add(requestResponseRabbitLogger);

            //loggers.Add(jsonFileLogger);

            loggers.Add(runtimeLogRepository);

            _logManager = new LogManager<RuntimeLogModel>(loggers);
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
        /// <param name="model">Yazılacak çalışma zamanı logun nesnesi</param>
        public async Task LogAsync(RuntimeLogModel model, CancellationTokenSource cancellationTokenSource)
        {
            await _logManager.LogAsync(model, cancellationTokenSource);
        }
    }
}
