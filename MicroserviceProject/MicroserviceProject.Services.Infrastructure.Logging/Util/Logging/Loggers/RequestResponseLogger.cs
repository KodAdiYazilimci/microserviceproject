using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Loggers;
using MicroserviceProject.Infrastructure.Logging.Managers;
using MicroserviceProject.Infrastructure.Logging.MongoDb.Loggers;
using MicroserviceProject.Services.Infrastructure.Logging.Configuration.Logging;
using MicroserviceProject.Services.Logging.Models;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging.Util.Logging.Loggers
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

            DefaultLogger<RequestResponseLogModel> defaultMongoLogger =
                new DefaultLogger<RequestResponseLogModel>(
                    new RequestResponseLogMongoConfiguration(configuration));

            loggers.Add(defaultMongoLogger);

            _logManager = new LogManager<RequestResponseLogModel>(loggers);
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak request-response log modeli</param>
        public async Task LogAsync(RequestResponseLogModel model, CancellationToken cancellationToken)
        {
            await _logManager.LogAsync(model, cancellationToken);
        }
    }
}
