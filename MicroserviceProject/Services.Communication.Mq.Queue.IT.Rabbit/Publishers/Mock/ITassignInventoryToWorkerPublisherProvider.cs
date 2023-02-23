using Services.Communication.Mq.Queue.IT.Configuration;

namespace Services.Communication.Mq.Queue.IT.Rabbit.Publishers.Mock
{
    /// <summary>
    /// Çalışana envanter atayan IT yayıncısını taklit eden sınıf
    /// </summary>
    public class ITAssignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan IT yayıncısını verir
        /// </summary>
        /// <param name="configuration">Çalışana envanter atayan IT yayıncısının yapılandırma sınıfı nesnesi</param>
        /// <returns></returns>
        public static ITAssignInventoryToWorkerPublisher GetPublisher(ITAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            return new ITAssignInventoryToWorkerPublisher(configuration);
        }
    }
}
