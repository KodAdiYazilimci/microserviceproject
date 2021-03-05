using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;

using Newtonsoft.Json;

using RabbitMQ.Client;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers
{
    /// <summary>
    /// Rabbit sunucusuna data üretecek sınıf
    /// </summary>
    /// <typeparam name="TModel">Data modelinin tipi</typeparam>
    public class Publisher<TModel> : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Data modelini üretmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private IRabbitConfiguration _rabbitConfiguration;

        /// <summary>
        /// Rabbit sunucusuna data üretecek varsayılan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Data modelini üretmek için rabbit sunucusunun yapılandırma ayarları</param>
        public Publisher(IRabbitConfiguration rabbitConfiguration)
        {
            _rabbitConfiguration = rabbitConfiguration;
        }

        /// <summary>
        /// Data üretimi yapar
        /// </summary>
        /// <param name="model">Üretilecek data nesnesi</param>
        /// <returns></returns>
        public virtual Task PublishAsync(TModel model, CancellationToken cancellationToken)
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
                    _rabbitConfiguration = null;
                }

                disposed = true;
            }
        }
    }
}
