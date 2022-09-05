using Infrastructure.Logging.File.Configuration;

using Microsoft.Extensions.Configuration;

using System.Text;

namespace Services.Api.Infrastructure.Logging.Configuration.Logging
{
    /// <summary>
    /// Request-response logları için dosya yapılandırma ayarları
    /// </summary>
    public class RequestResponseLogFileConfiguration : IFileConfiguration
    {
        /// <summary>
        /// Request-response logları için dosya yapılandırma ayarları
        /// </summary>
        /// <param name="configuration">Request-response log ayarlarının çekileceği configuration</param>
        public RequestResponseLogFileConfiguration(IConfiguration configuration)
        {
            RelativePath = configuration
               .GetSection("Configuration")
               .GetSection("Logging")
               .GetSection("RequestResponseLogging")
               .GetSection("FileConfiguration")["RelativePath"];

            AbsolutePath = configuration
               .GetSection("Configuration")
               .GetSection("Logging")
               .GetSection("RequestResponseLogging")
               .GetSection("FileConfiguration")["AbsolutePath"];

            FileName = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")["FileName"];

            string encodingCode = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")["Encoding"];

            Encoding = Encoding.GetEncoding(encodingCode);
        }

        /// <summary>
        /// Yazılacak log dosyasının kesin konumu
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Yazılacak log dosyasının göreceli konumu
        /// </summary>
        public string RelativePath { get; set; }

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
