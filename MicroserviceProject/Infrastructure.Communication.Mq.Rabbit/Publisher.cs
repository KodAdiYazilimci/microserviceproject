using Infrastructure.Communication.Mq.Abstraction;
using Infrastructure.Communication.Mq.Configuration;

using Newtonsoft.Json;

using RabbitMQ.Client;

using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Mq.Rabbit
{
    /// <summary>
    /// Rabbit sunucusuna data üretecek sınıf
    /// </summary>
    /// <typeparam name="TModel">Data modelinin tipi</typeparam>
    public class Publisher<TModel> : IPublisher<TModel>, IDisposable where TModel : class
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Data modelini üretmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private IRabbitConfiguration _rabbitConfiguration;

        private IModel channel = null;
        private IConnection connection = null;

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
        public virtual Task PublishAsync(TModel model, CancellationTokenSource cancellationTokenSource)
        {
            if (connection == null || channel == null)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = _rabbitConfiguration.Host,
                    UserName = _rabbitConfiguration.UserName,
                    Password = _rabbitConfiguration.Password
                };

                connection = factory.CreateConnection();

                channel = connection.CreateModel();

            }
            string jsonLog = JsonConvert.SerializeObject(model);

            byte[] jsonBuffer = UTF8Encoding.UTF8.GetBytes(jsonLog);

            channel.BasicPublish(exchange: "", routingKey: _rabbitConfiguration.QueueName, mandatory: true, basicProperties: null, body: jsonBuffer);

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
                    if (channel != null)
                    {
                        channel.Dispose();
                        channel = null;
                    }

                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }

                    _rabbitConfiguration = null;
                }

                disposed = true;
            }
        }
    }
}
