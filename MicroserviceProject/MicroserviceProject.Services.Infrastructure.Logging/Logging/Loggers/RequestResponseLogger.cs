using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Loggers;
using MicroserviceProject.Infrastructure.Logging.Managers;
using MicroserviceProject.Model.Logging;
using MicroserviceProject.Services.Infrastructure.Logging.Logging.Configuration;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;

namespace MicroserviceProject.Services.Infrastructure.Logging.Logging.Loggers
{
    /// <summary>
    /// Request-Response loglarını yazan sınıf
    /// </summary>
    public class RequestResponseLogger
    {
        /// <summary>
        /// Log yazma yönetimini gerçekleştiren sınıf
        /// </summary>
        private readonly LogManager<RequestResponseLogModel> _logManager;

        /// <summary>
        /// Request-Response loglarını yazan sınıf
        /// <paramref name="configuration">Request-response log ayarlarının çekileceği configuration</paramref>
        /// </summary>
        public RequestResponseLogger(IConfiguration configuration)
        {
            List<ILogger<RequestResponseLogModel>> loggers = new List<ILogger<RequestResponseLogModel>>();

            JsonFileLogger<RequestResponseLogModel> jsonFileLogger =
                new JsonFileLogger<RequestResponseLogModel>(
                    new RequestResponseLogFileConfiguration(configuration));

            loggers.Add(jsonFileLogger);

            _logManager = new LogManager<RequestResponseLogModel>(loggers);
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="requestResponseLog">Yazılacak request-response log modeli</param>
        public void Log(RequestResponseLogModel requestResponseLog)
        {
            _logManager.Log(requestResponseLog);
        }
    }
}
