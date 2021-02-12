using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;

using Newtonsoft.Json;

using RabbitMQ.Client;

using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers
{
    /// <summary>
    /// Rabbit sunucusuna log üretecek varsayılan sınıf
    /// </summary>
    /// <typeparam name="TModel">Log modelinin tipi</typeparam>
    public class DefaultLogProducer<TModel> : Publisher<TModel>, ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log modelini üretmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private readonly IRabbitConfiguration _rabbitConfiguration;

        /// <summary>
        /// Rabbit sunucusuna log üretecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini üretmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogProducer(IRabbitConfiguration rabbitConfiguration) : base(rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        /// <summary>
        /// Log üretimi yapar
        /// </summary>
        /// <param name="model">Üretilecek logun nesnesi</param>
        /// <returns></returns>
        public Task LogAsync(TModel model)
        {
            return base.PublishAsync(model);
        }
    }
}
