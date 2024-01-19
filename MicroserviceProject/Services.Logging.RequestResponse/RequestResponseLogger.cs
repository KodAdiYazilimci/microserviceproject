using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Elastic.Loggers;
using Infrastructure.Logging.File.Loggers;
using Infrastructure.Logging.Managers;
using Infrastructure.Logging.RabbitMq.Producers;

using Microsoft.Extensions.Configuration;

using Services.Logging.RequestResponse.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Logging.RequestResponse
{
    /// <summary>
    /// Request-response loglarını yazan sınıf
    /// </summary>
    public class RequestResponseLogger : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log yazma işlemlerini yürütecek yönetici
        /// </summary>
        private readonly BulkLogManager<RequestResponseLogModel> _logManager;

        private List<RequestResponseLogModel> responseLogModels = new List<RequestResponseLogModel>();

        /// <summary>
        /// Request-response loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Request-response log ayarlarının çekileceği configuration</param>
        public RequestResponseLogger(IConfiguration configuration)
        {
            List<IBulkLogger<RequestResponseLogModel>> loggers = new List<IBulkLogger<RequestResponseLogModel>>();

            BulkJsonFileLogger<RequestResponseLogModel> jsonFileLogger =
                new BulkJsonFileLogger<RequestResponseLogModel>(
                    new RequestResponseLogFileConfiguration(configuration));

            loggers.Add(jsonFileLogger);

            DefaultBulkLogProducer<RequestResponseLogModel> requestResponseRabbitLogger =
                new DefaultBulkLogProducer<RequestResponseLogModel>(
                    new RequestResponseLogRabbitConfiguration(configuration));

            loggers.Add(requestResponseRabbitLogger);

            BulkElasticLogger<RequestResponseLogModel> elasticLogger =
                new BulkElasticLogger<RequestResponseLogModel>(
                    new RequestResponseLogElasticConfiguration(configuration));

            loggers.Add(elasticLogger);

            _logManager = new BulkLogManager<RequestResponseLogModel>(loggers);
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

                    if (responseLogModels != null)
                    {
                        responseLogModels.Clear();
                        responseLogModels = null;
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak request-response logun nesnesi</param>
        public async Task LogAsync(RequestResponseLogModel model, CancellationTokenSource cancellationTokenSource)
        {
            await _logManager.LogAsync(new List<RequestResponseLogModel>() { model }, cancellationTokenSource);
        }
    }
}
