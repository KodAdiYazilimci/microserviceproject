
using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit.Buying
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
    /// </summary>
    public class CreateInventoryRequestRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Satınalma departmanına alınması istenilen envanter talepleri için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public CreateInventoryRequestRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("Buying")
                .GetSection("QueueNames")
                .GetSection("CreateInventoryRequest").Value;
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
