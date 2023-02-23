using Services.Communication.Mq.Queue.AA.Configuration;

namespace Services.Communication.Mq.Queue.AA.Rabbit.Publishers.Mock
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncısını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncısını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma araüyüz nesnesi</param>
        /// <returns></returns>
        public static AAAssignInventoryToWorkerPublisher GetPublisher(AAAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            return new AAAssignInventoryToWorkerPublisher(configuration);
        }
    }
}
