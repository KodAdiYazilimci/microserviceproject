using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Logging.RabbitMq.Producers;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Mq.Rabbit.Publisher
{
    /// <summary>
    /// Rabbit kuyruğuna kayıt ekleyecek sınıfların temel sınıfı
    /// </summary>
    /// <typeparam name="TModel">Kuyruğa eklenecek kaydın tipi</typeparam>
    public abstract class BasePublisher<TModel> : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Data üretici sınıfın nesnesi
        /// </summary>
        private Publisher<TModel> _publisher;

        /// <summary>
        /// Rabbit kuyruğuna kayıt edilecek nesnelerin tampon belleği
        /// </summary>
        private List<TModel> buffer;

        /// <summary>
        /// Rabbit kuyruğuna kayıt ekleyecek sınıfların temel sınıfı
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public BasePublisher(IRabbitConfiguration rabbitConfiguration)
        {
            buffer = new List<TModel>();

            _publisher = new Publisher<TModel>(rabbitConfiguration);
        }

        /// <summary>
        /// Rabbit kuyruğuna kayıt edilecek nesneleri tampon belleğe ekler
        /// </summary>
        /// <param name="model">Eklenecek kaydın nesnesi</param>
        public virtual void AddToBuffer(TModel model)
        {
            buffer.Add(model);
        }

        /// <summary>
        /// Rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="model">Eklenecek kaydın nesnesi</param>
        /// <returns></returns>
        public virtual Task PublishAsync(TModel model, CancellationTokenSource cancellationTokenSource)
        {
            return _publisher.PublishAsync(model, cancellationTokenSource);
        }

        /// <summary>
        /// Tampon belleğe eklenen kayıtları rabbit kuyruğuna atar
        /// </summary>
        /// <param name="cancellationTokenSource"></param>
        /// <returns></returns>
        public virtual async Task PublishBufferAsync(CancellationTokenSource cancellationTokenSource)
        {
            foreach (var data in buffer)
            {
                await _publisher.PublishAsync(data, cancellationTokenSource);
            }

            buffer.Clear();
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
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    if (_publisher != null)
                    {
                        _publisher.Dispose();
                        _publisher = null;
                    }

                    if (buffer != null)
                    {
                        buffer.Clear();
                        buffer = null;
                    }
                }

                disposed = true;
            }
        }
    }
}
