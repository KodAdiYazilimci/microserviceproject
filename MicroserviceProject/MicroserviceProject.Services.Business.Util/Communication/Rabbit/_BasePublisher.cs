using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;
using MicroserviceProject.Infrastructure.Logging.RabbitMq.Producers;

using System.Threading.Tasks;

namespace MicroserviceProject.Services.Business.Util.Communication.Rabbit
{
    /// <summary>
    /// Rabbit kuyruğuna kayıt ekleyecek sınıfların temel sınıfı
    /// </summary>
    /// <typeparam name="TModel">Kuyruğa eklenecek kaydın tipi</typeparam>
    public abstract class BasePublisher<TModel>
    {
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
        public virtual Task PublishAsync(TModel model)
        {
            return _publisher.PublishAsync(model);
        }
    }
}
