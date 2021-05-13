using Infrastructure.Communication.Mq.Rabbit.Configuration.IT;
using Infrastructure.Communication.Mq.Rabbit.Publisher.IT;

namespace Infrastructure.Mock.Publishers
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısını taklit eden sınıf
    /// </summary>
    public class ITassignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısı
        /// </summary>
        private static ITAssignInventoryToWorkerPublisher publisher = null;

        /// <summary>
        /// Çalışana envanter atayan IT yayıncısını verir
        /// </summary>
        /// <param name="configuration">Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesi</param>
        /// <returns></returns>
        public static ITAssignInventoryToWorkerPublisher GetPublisher(ITAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new ITAssignInventoryToWorkerPublisher(configuration);
            }

            return publisher;
        }
    }
}
