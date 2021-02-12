
using Newtonsoft.Json;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System.Text;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Communication.Mq.Rabbit
{
    /// <summary>
    /// Rabbit sunucusundaki dataları tüketecek varsayılan sınıf
    /// </summary>
    /// <typeparam name="TModel">Data modelinin tipi</typeparam>
    public class Consumer<TModel>
    {
        /// <summary>
        /// Data tüketildiğinde ateşlenecek olayı handler ı
        /// </summary>
        /// <param name="data">Data modelinin nesnesi</param>
        /// <returns></returns>
        public delegate Task OnConsumeHandlerAsync(TModel data);

        /// <summary>
        /// Data tüketildiği anda ateşlenecek olay
        /// </summary>
        public event OnConsumeHandlerAsync OnConsumed;

        /// <summary>
        /// Data modelini tüketmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private readonly IRabbitConfiguration _rabbitConfiguration;

        /// <summary>
        /// Rabbit sunucusundaki dataları tüketecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Data modelini tüketmek için rabbit sunucusunun yapılandırma ayarları</param>
        public Consumer(IRabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        /// <summary>
        /// Data tüketimini başlatır
        /// </summary>
        public virtual void StartToConsume()
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
