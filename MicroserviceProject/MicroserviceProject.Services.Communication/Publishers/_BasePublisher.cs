using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Communication.Publishers
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
        protected bool Disposed = false;

        /// <summary>
        /// Data üretici sınıfın nesnesi
        /// </summary>
        private Publisher<TModel> _publisher;

        /// <summary>
        /// Rabbit kuyruğuna kayıt ekleyecek sınıfların temel sınıfı
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public BasePublisher(IRabbitConfiguration rabbitConfiguration)
        {
            _publisher = new Publisher<TModel>(rabbitConfiguration);
        }

        /// <summary>
        /// Rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="model">Eklenecek kaydın nesnesi</param>
        /// <returns></returns>
        public virtual Task PublishAsync(TModel model, CancellationToken cancellationToken)
        {
            return _publisher.PublishAsync(model, cancellationToken);
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
                if (!Disposed)
                {
                    if (_publisher != null)
                    {
                        _publisher.Dispose();
                        _publisher = null;
                    }
                }

                Disposed = true;
            }
        }
    }
}
