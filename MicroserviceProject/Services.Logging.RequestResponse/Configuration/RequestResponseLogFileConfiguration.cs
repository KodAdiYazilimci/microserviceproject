using Infrastructure.Logging.File.Configuration;

using Microsoft.Extensions.Configuration;

using System;
using System.Text;

namespace Services.Logging.RequestResponse.Configuration
{
    /// <summary>
    /// Request-response logları için dosya yapılandırma ayarları
    /// </summary>
    public class RequestResponseLogFileConfiguration : IFileConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
                .GetSection("FileConfiguration")["Path"];

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
                    Path = string.Empty;
                    FileName = string.Empty;
                    Encoding = null;
                }

                disposed = true;
            }
        }
    }
}
