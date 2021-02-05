using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.Model;
using MicroserviceProject.Infrastructure.Logging.RabbitMq.Configuration;

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
    public class DefaultLogProducer<TModel> : ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Log modelini üretmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private readonly IRabbitConfiguration _rabbitConfiguration;

        /// <summary>
        /// Rabbit sunucusuna log üretecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Log modelini üretmek için rabbit sunucusunun yapılandırma ayarları</param>
        public DefaultLogProducer(IRabbitConfiguration rabbitConfiguration)
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
            var factory = new ConnectionFactory()
            {
                HostName = _rabbitConfiguration.Host,
                UserName = _rabbitConfiguration.UserName,
                Password = _rabbitConfiguration.Password
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    QueueDeclareOk queue = channel.QueueDeclare(queue: _rabbitConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    string jsonLog = JsonConvert.SerializeObject(model);

                    byte[] jsonBuffer = UTF8Encoding.UTF8.GetBytes(jsonLog);

                    channel.BasicPublish(exchange: "", routingKey: _rabbitConfiguration.QueueName, mandatory: true, basicProperties: null, body: jsonBuffer);
                }
            }

            return Task.CompletedTask;
        }
    }
}
