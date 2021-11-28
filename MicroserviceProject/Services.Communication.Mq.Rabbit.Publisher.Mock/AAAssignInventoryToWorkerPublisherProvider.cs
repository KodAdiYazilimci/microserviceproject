using Services.Communication.Mq.Rabbit.Configuration.Department.AA;
using Services.Communication.Mq.Rabbit.Publisher.Department.AA;

namespace Services.Communication.Mq.Rabbit.Publisher.Mock
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
