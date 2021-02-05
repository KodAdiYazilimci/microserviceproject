using MicroserviceProject.Infrastructure.Logging.Model;
using MicroserviceProject.Infrastructure.Logging.RabbitMq.Configuration;

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
    public class DefaultLogConsumer<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log tüketildiğinde ateşlenecek olayı handler ı
        /// </summary>
        /// <param name="data">Log modelinin nesnesi</param>
        /// <returns></returns>
        public delegate Task OnConsumeHandlerAsync(TModel data);

        /// <summary>
        /// Log tüketildiği anda ateşlenecek olay
        /// </summary>
        public event OnConsumeHandlerAsync OnConsumed;

        /// <summary>
        /// Log modelini tüketmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private readonly IRabbitConfiguration _rabbitConfiguration;

        /// <summary>
        /// Rabbit sunucusundaki logları tüketecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini tüketmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogConsumer(IRabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        /// <summary>
        /// Log tüketimini başlatır
        /// </summary>
        public void StartToConsume()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitConfiguration.Host,
                UserName = _rabbitConfiguration.UserName,
                Password = _rabbitConfiguration.Password
            };

            IConnection connection = factory.CreateConnection();

            IModel channel = connection.CreateModel();

            QueueDeclareOk declare = channel.QueueDeclare(queue: _rabbitConfiguration.QueueName,
                       durable: false,
                       exclusive: false,
                       autoDelete: false,
                       arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (obj, args) =>
            {
                var jsonObject = Encoding.UTF8.GetString(args.Body.ToArray());

                if (this.OnConsumed != null)
                {
                    this.OnConsumed(JsonConvert.DeserializeObject<TModel>(jsonObject));
                }
            };

            channel.BasicConsume(_rabbitConfiguration.QueueName, autoAck: true, consumer);
        }
    }
}
