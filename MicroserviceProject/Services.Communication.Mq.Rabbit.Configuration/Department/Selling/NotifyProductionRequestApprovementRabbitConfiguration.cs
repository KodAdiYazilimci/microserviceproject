
using Microsoft.Extensions.Configuration;

using System;

namespace Services.Communication.Mq.Rabbit.Configuration.Department.Selling
{
    /// <summary>
    /// Üretilmesi talep edilmiş ürünlerin onay sonuçlarını kuyruğa eklemesi için yapılandırma sınıfı
    /// </summary>
    public class NotifyProductionRequestApprovementRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Üretilmesi talep edilmiş ürünlerin onay sonuçlarını kuyruğa eklemesi için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public NotifyProductionRequestApprovementRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Selling")
                .GetSection("QueueNames")["NotifyProductionRequestApprovement"];
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }
    }
}
