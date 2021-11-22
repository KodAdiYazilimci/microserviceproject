using Services.Communication.Mq.Rabbit.Configuration.Department.AA;
using Services.Communication.Mq.Rabbit.Publisher.Department.AA;

namespace Infrastructure.Mock.Publishers
{
    /// <summary>
    /// Çalışana envanter atayan idari işler yayıncısını taklit eden sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerPublisherProvider
    {
        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncısı
        /// </summary>
        private static AAAssignInventoryToWorkerPublisher publisher = null;

        /// <summary>
        /// Çalışana envanter atayan idari işler yayıncısını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma araüyüz nesnesi</param>
        /// <returns></returns>
        public static AAAssignInventoryToWorkerPublisher GetPublisher(AAAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new AAAssignInventoryToWorkerPublisher(configuration);
            }

            return publisher;
        }
    }
}
