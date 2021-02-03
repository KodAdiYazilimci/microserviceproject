using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Loggers;
using MicroserviceProject.Infrastructure.Logging.Managers;
using MicroserviceProject.Model.Logging;
using MicroserviceProject.Services.Security.Authorization.Configuration.Logging;

using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization.Util.Logging.Loggers
{
    public class RequestResponseLogger
    {
        private readonly LogManager<RequestResponseLogModel> _logManager;

        public RequestResponseLogger(IConfiguration configuration)
        {
            List<ILogger<RequestResponseLogModel>> loggers = new List<ILogger<RequestResponseLogModel>>();

            JsonFileLogger<RequestResponseLogModel> jsonFileLogger =
                new JsonFileLogger<RequestResponseLogModel>(
                    new DefaultFileConfiguration(configuration));

            loggers.Add(jsonFileLogger);

            _logManager = new LogManager<RequestResponseLogModel>(loggers);
        }

        public void Log(RequestResponseLogModel model)
        {
            _logManager.Log(model);
        }
    }
}
