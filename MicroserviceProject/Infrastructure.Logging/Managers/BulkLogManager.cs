using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.Model;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Logging.Managers
{
    /// <summary>
    /// Log yazma işlemlerini yürüten sınıf
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class BulkLogManager<TModel> : IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Log yazacak logger sınıfları
        /// </summary>
        private readonly List<IBulkLogger<TModel>> _loggers;

        /// <summary>
        /// Log yazma işlemlerini yürüten sınıf
        /// </summary>
        /// <param name="loggers">Log yazacak logger sınıfları</param>
        public BulkLogManager(List<IBulkLogger<TModel>> loggers)
        {
            _loggers = loggers;
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
            if (disposed)
            {
                if (!disposed)
                {
                    if (_loggers != null)
                    {
                        _loggers.Clear();
                    }
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Log yazar
        /// </summary>
        /// <param name="models">Yazılacak logun modeli</param>
        public Task LogAsync(List<TModel> models, CancellationTokenSource cancellationTokenSource)
        {
            Task[] tasks = new Task[_loggers.Count];

            for (int i = 0; i < _loggers.Count; i++)
            {
                tasks[i] = _loggers[i].LogAsync(models, cancellationTokenSource);
            }

            Task.WaitAll(tasks);

            return Task.CompletedTask;
        }
    }
}
