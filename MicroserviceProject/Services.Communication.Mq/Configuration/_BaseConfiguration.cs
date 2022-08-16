
using Infrastructure.Communication.Mq.Configuration;

using Microsoft.Extensions.Configuration;

using System.Diagnostics;

namespace Services.Communication.Mq.Configuration
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
                Convert.ToBoolean(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["EnviromentVariableNamePrefix"] + "_DefaultHost")
                :
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Host")["DefaultHost"];

            UserName =
                Convert.ToBoolean(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["EnviromentVariableNamePrefix"] + "_DefaultUserName")
                :
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Host")["DefaultUserName"];

            Password =
                Convert.ToBoolean(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    configuration
                    .GetSection("Configuration")
                    .GetSection("RabbitQueues")
                    .GetSection("Host")["EnviromentVariableNamePrefix"] + "_DefaultPassword")
                :
                configuration
                .GetSection("Configuration")
                .GetSection("RabbitQueues")
                .GetSection("Host")["DefaultPassword"];
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
