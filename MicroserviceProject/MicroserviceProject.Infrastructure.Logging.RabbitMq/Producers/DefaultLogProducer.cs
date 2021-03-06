using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;

using Newtonsoft.Json;

using RabbitMQ.Client;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers
{
    /// <summary>
    /// Rabbit sunucusuna log üretecek varsayılan sınıf
    /// </summary>
    /// <typeparam name="TModel">Log modelinin tipi</typeparam>
    public class DefaultLogProducer<TModel> : Publisher<TModel>, ILogger<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit sunucusuna log üretecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini üretmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogProducer(IRabbitConfiguration rabbitConfiguration) : base(rabbitConfiguration)
        {

        }

        /// <summary>
        /// Log üretimi yapar
        /// </summary>
        /// <param name="model">Üretilecek logun nesnesi</param>
        /// <returns></returns>
        public Task LogAsync(TModel model, CancellationToken cancellationToken)
        {
            return base.PublishAsync(model, cancellationToken);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {

                }

                disposed = true;

                Dispose();
            }
        }
    }
}
