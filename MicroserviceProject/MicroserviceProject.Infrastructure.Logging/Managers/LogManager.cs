using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Model.Logging;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.Managers
{
    /// <summary>
    /// Log yazma işlemlerini yürüten sınıf
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class LogManager<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log yazacak logger sınıfları
        /// </summary>
        private List<ILogger<TModel>> _loggers;

        /// <summary>
        /// Log yazma işlemlerini yürüten sınıf
        /// </summary>
        /// <param name="loggers">Log yazacak logger sınıfları</param>
        public LogManager(List<ILogger<TModel>> loggers)
        {
            _loggers = loggers;
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
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
