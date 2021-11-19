
using Microsoft.Extensions.Configuration;

using System;

namespace Communication.Mq.Rabbit.Configuration.Department.Buying
{
    /// <summary>
    /// Satın alınması planlanan envanterlere ait bütçenin sonuçlandırılması için yapılandırma sınıfı
    /// </summary>
    public class NotifyCostApprovementRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Satın alınması planlanan envanterlere ait bütçenin sonuçlandırılması için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public NotifyCostApprovementRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Buying")
                .GetSection("QueueNames")["NotifyCostApprovement"];
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
