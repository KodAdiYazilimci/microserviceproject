
using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit.AA
{
    /// <summary>
    /// Envanter talebiyle ilgili satınalma sonucunu rabbit kuyruğundan almak için yapılandırma sınıfı
    /// </summary>
    public class AAInformInventoryRequestRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu rabbit kuyruğundan almak için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public AAInformInventoryRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("AA")
                .GetSection("QueueNames")
                .GetSection("InformInventoryRequest").Value;
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Disposed)
                {
                    Disposed = true;
                }
            }
        }
    }
}
