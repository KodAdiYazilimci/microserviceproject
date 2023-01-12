using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.File.Loggers;
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
        private readonly BulkLogManager<RuntimeLogModel> _logManager;

        private List<RuntimeLogModel> runtimeLogModels = new List<RuntimeLogModel>();

        /// <summary>
        /// Çalışma zamanı loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Çalışma zamanı log ayarlarının çekileceği configuration</param>
        public RuntimeLogger(IConfiguration configuration)
        {
            List<IBulkLogger<RuntimeLogModel>> loggers = new List<IBulkLogger<RuntimeLogModel>>();

            //BulkJsonFileLogger<RuntimeLogModel> jsonFileLogger =
            //    new BulkJsonFileLogger<RuntimeLogModel>(
            //        new RuntimeLogFileConfiguration(configuration));

            DefaultBulkLogProducer<RuntimeLogModel> requestResponseRabbitLogger =
                new DefaultBulkLogProducer<RuntimeLogModel>(
                    new RuntimeLogRabbitConfiguration(configuration));

            BulkRuntimeLogRepository runtimeLogRepository = new BulkRuntimeLogRepository(configuration);

            loggers.Add(requestResponseRabbitLogger);

            //loggers.Add(jsonFileLogger);

            loggers.Add(runtimeLogRepository);

            _logManager = new BulkLogManager<RuntimeLogModel>(loggers);
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

                    if (runtimeLogModels != null)
                    {
                        runtimeLogModels.Clear();
                        runtimeLogModels = null;
                    }
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
            if (runtimeLogModels.Count > 100)
            {
                RuntimeLogModel[] tempLogModels = new RuntimeLogModel[runtimeLogModels.Count];
                runtimeLogModels.CopyTo(tempLogModels);
                runtimeLogModels.Clear();

                await _logManager.LogAsync(tempLogModels.ToList(), cancellationTokenSource);
            }
            else
            {
                runtimeLogModels.Add(model);
            }
        }
    }
}
