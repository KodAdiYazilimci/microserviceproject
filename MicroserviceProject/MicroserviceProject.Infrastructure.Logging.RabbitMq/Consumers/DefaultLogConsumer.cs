using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.Model;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.RabbitMq.Consumers
{
    /// <summary>
    /// Rabbit sunucusundaki logları tüketecek varsayılan sınıf
    /// </summary>
    /// <typeparam name="TModel">Log modelinin tipi</typeparam>
    public class DefaultLogConsumer<TModel> : Consumer<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit sunucusundaki logları tüketecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini tüketmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogConsumer(IRabbitConfiguration rabbitConfiguration) : base(rabbitConfiguration)
        {
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
