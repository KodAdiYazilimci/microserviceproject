
using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit.AA
{
    /// <summary>
    /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
    /// </summary>
    public class AAAssignInventoryToWorkerRabbitConfiguration : BaseConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışana envanter ekleyecek rabbit kuyruğu için yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public AAAssignInventoryToWorkerRabbitConfiguration(IConfiguration configuration)
            : base(configuration)
        {
            QueueName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Services")
                .GetSection("AA")
                .GetSection("QueueNames")
                .GetSection("AssignInventoryToWorker").Value;
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

                }

                disposed = true;

                Dispose();
            }
        }
    }
}
