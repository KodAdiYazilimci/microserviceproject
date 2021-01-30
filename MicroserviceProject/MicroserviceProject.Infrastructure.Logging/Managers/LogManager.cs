using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Model.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.Managers
{
    public class LogManager<TModel> where TModel : BaseLogModel, new()
    {
        public List<ILogger<TModel>> _loggers;

        public LogManager(List<ILogger<TModel>> loggers)
        {
            _loggers = loggers;
        }

        public void Log(TModel model)
        {
            Task[] tasks = new Task[_loggers.Count];

            for (int i = 0; i < _loggers.Count; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    _loggers[i].Log(model);
                });
            }

            Task.WaitAll(tasks);
        }
    }
}
