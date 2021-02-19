using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.Managers
{
    /// <summary>
    /// Log yazma işlemlerini yürüten sınıf
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class LogManager<TModel> : IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log yazacak logger sınıfları
        /// </summary>
        private readonly List<ILogger<TModel>> _loggers;

        /// <summary>
        /// Log yazma işlemlerini yürüten sınıf
        /// </summary>
        /// <param name="loggers">Log yazacak logger sınıfları</param>
        public LogManager(List<ILogger<TModel>> loggers)
        {
            _loggers = loggers;
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
            if (disposed)
            {
                if (!disposed)
                {
                    _loggers.Clear();
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        public Task LogAsync(TModel model, CancellationToken cancellationToken)
        {
            Task[] tasks = new Task[_loggers.Count];

            for (int i = 0; i < _loggers.Count; i++)
            {
                tasks[i] = _loggers[i].LogAsync(model, cancellationToken);
            }

            Task.WaitAll(tasks);

            return Task.CompletedTask;
        }
    }
}
