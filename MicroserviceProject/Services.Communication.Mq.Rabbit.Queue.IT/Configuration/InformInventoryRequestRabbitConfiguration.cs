
using Microsoft.Extensions.Configuration;

using Services.Communication.Mq.Rabbit.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.IT.Configuration
{
    /// <summary>
    /// Envanter talebiyle ilgili satınalma sonucunu rabbit kuyruğundan almak için yapılandırma sınıfı
    /// </summary>
    public class InformInventoryRequestRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu rabbit kuyruğundan almak için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public InformInventoryRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("IT")
                .GetSection("QueueNames")["InformInventoryRequest"];
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
