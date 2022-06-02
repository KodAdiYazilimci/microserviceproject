using Services.Communication.Mq.Queue.AA.Configuration;

namespace Services.Communication.Mq.Queue.AA.Rabbit.Publishers.Mock
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncısını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncısı
        /// </summary>
        private static AssignInventoryToWorkerPublisher publisher = null;

        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncısını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma araüyüz nesnesi</param>
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
