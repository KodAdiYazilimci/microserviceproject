using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.Model;

using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.RabbitMq.Consumers
{
    /// <summary>
    /// Rabbit sunucusundaki logları tüketecek varsayılan sınıf
    /// </summary>
    /// <typeparam name="TModel">Log modelinin tipi</typeparam>
    public class DefaultLogConsumer<TModel> : Consumer<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Rabbit sunucusundaki logları tüketecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini tüketmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogConsumer(IRabbitConfiguration rabbitConfiguration) : base(rabbitConfiguration)
        {
        }
    }
}
