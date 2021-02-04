using MicroserviceProject.Infrastructure.Logging.File.Configuration;

using Microsoft.Extensions.Configuration;

using System.Text;

namespace MicroserviceProject.Services.Security.Authorization.Configuration.Logging
{
    /// <summary>
    /// Request-response logları için dosya yapılandırma ayarları
    /// </summary>
    public class RequestResponseLogFileConfiguration : IFileConfiguration
    {
        /// <summary>
        /// Request-response logları için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        /// <param name="configuration">Request-response log ayarlarının çekileceği configuration</param>
        public RequestResponseLogFileConfiguration(IConfiguration configuration)
        {
            Path = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("Path").Value;

            FileName = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("FileName").Value;

            string encodingCode = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("Encoding").Value;

            Encoding = Encoding.GetEncoding(encodingCode);
        }

        /// <summary>
        /// Yazılacak log dosyasının konumu
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Yazılacak log dosyasının adı
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Yazılacak log dosyasının kodlaması
        /// </summary>
        public Encoding Encoding { get; set; }
    }
}
