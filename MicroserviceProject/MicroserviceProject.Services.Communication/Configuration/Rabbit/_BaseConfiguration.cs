using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit;

using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Services.Communication.Configuration.Rabbit
{
    /// <summary>
    /// Rabbit kuyrukları için temel yapılandırma sınıfı
    /// </summary>
    public class BaseConfiguration : IRabbitConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyrukları için temel yapılandırma sınıfı
        /// <paramref name="configuration">Ayarların okunacağı configuration nesnesi</paramref>
        /// </summary>
        /// <param name="configuration"></param>
        public BaseConfiguration(IConfiguration configuration)
        {
            Host =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultHost").Value;

            UserName =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultUserName").Value;

            Password =
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("DefaultPassword").Value;
        }

        /// <summary>
        /// Rabbit sunucusunun adı
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kullanıcı adı
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Rabbit sunucusunun parolası
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Rabbit sunucusunun kuyruk adı
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    Host = string.Empty;
                    UserName = string.Empty;
                    Password = string.Empty;
                    QueueName = string.Empty;
                }

                disposed = true;
            }
        }
    }
}
