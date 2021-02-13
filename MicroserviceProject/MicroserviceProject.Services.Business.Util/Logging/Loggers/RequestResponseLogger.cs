using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Loggers;
using MicroserviceProject.Infrastructure.Logging.Managers;
using MicroserviceProject.Infrastructure.Logging.Model;
using MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers;
using MicroserviceProject.Services.Business.Configuration.Logging;
using MicroserviceProject.Services.Configuration.Logging;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Services.Business.Util.Logging.Loggers
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
        private readonly LogManager<RequestResponseLogModel> _logManager;

        /// <summary>
        /// Request-response loglarını yazan sınıf
        /// </summary>
        /// <param name="configuration">Request-response log ayarlarının çekileceği configuration</param>
        public RequestResponseLogger(IConfiguration configuration)
        {
            List<ILogger<RequestResponseLogModel>> loggers = new List<ILogger<RequestResponseLogModel>>();

            JsonFileLogger<RequestResponseLogModel> jsonFileLogger =
                new JsonFileLogger<RequestResponseLogModel>(
                    new RequestResponseLogFileConfiguration(configuration));

            DefaultLogProducer<RequestResponseLogModel> requestResponseRabbitLogger =
                new DefaultLogProducer<RequestResponseLogModel>(
                    new RequestResponseLogRabbitConfiguration(configuration));

            loggers.Add(requestResponseRabbitLogger);

            loggers.Add(jsonFileLogger);

            _logManager = new LogManager<RequestResponseLogModel>(loggers);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
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
                    _logManager.Dispose();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak request-response logun nesnesi</param>
        public void Log(RequestResponseLogModel model)
        {
            _logManager.Log(model);
        }
    }
}
