using Services.Communication.Mq.Queue.IT.Configuration;

namespace Services.Communication.Mq.Queue.IT.Rabbit.Publishers.Mock
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısını taklit eden sınıf
    /// </summary>
    public class ITassignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısı
        /// </summary>
        private static AssignInventoryToWorkerPublisher publisher = null;

        /// <summary>
        /// Çalışana envanter atayan IT yayıncısını verir
        /// </summary>
        /// <param name="configuration">Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesi</param>
        /// <returns></returns>
        public static AssignInventoryToWorkerPublisher GetPublisher(AssignInventoryToWorkerRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new AssignInventoryToWorkerPublisher(configuration);
            }

            return publisher;
        }
    }
}
