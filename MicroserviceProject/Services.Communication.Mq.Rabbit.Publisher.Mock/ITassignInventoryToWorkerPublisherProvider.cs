﻿using Services.Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.Communication.Mq.Rabbit.Publisher.Department.IT;

namespace Services.Communication.Mq.Rabbit.Publisher.Mock
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
