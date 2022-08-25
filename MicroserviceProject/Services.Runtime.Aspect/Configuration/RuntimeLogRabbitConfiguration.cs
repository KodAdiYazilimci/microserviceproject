using Infrastructure.Communication.Mq.Configuration;

using Microsoft.Extensions.Configuration;

using System.Diagnostics;

namespace Services.Logging.Aspect.Configuration
{
    /// <summary>
    /// Çalışma zamanı logları için rabbit sunucusunun yapılandırma ayarları
    /// </summary>
    public class RuntimeLogRabbitConfiguration : IRabbitConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Request-response log ayarlarının çekileceği configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Çalışma zamanı logları için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        /// <param name="configuration">Çalışma zamanı log ayarlarının çekileceği configuration</param>
        public RuntimeLogRabbitConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;

            Host =
                Convert.ToBoolean(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["EnvironmentVariableNamePrefix"] + "_Host")
                :
                _configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("RabbitConfiguration")["Host"];

            UserName =
                Convert.ToBoolean(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["EnvironmentVariableNamePrefix"] + "_UserName")
                :
                _configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("RabbitConfiguration")["UserName"];

            Password =
                Convert.ToBoolean(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                ?
                Environment.GetEnvironmentVariable(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RuntimeLogging")
                    .GetSection("RabbitConfiguration")["EnvironmentVariableNamePrefix"] + "_Password")
                :
                _configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("RabbitConfiguration")["Password"];

            QueueName =
                _configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("RabbitConfiguration")["QueueName"];
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
        public void Dispose(bool disposing)
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
