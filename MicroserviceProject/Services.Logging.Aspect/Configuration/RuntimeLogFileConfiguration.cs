using Infrastructure.Logging.File.Configuration;

using Microsoft.Extensions.Configuration;

using System.Text;

namespace Services.Logging.Aspect.Configuration
{
    /// <summary>
    /// Çalışma zamanı logları için dosya yapılandırma ayarları
    /// </summary>
    public class RuntimeLogFileConfiguration : IFileConfiguration, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışma zamanı logları için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        /// <param name="configuration">Çalışma zamanı log ayarlarının çekileceği configuration</param>
        public RuntimeLogFileConfiguration(IConfiguration configuration)
        {
            Path = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("FileConfiguration")["Path"];

            FileName = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("FileConfiguration")["FileName"];

            string encodingCode = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
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
