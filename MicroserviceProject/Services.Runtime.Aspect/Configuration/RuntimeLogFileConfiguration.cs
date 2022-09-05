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
            RelativePath = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("FileConfiguration")["RelativePath"];

            AbsolutePath = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RuntimeLogging")
                .GetSection("FileConfiguration")["AbsolutePath"];

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
                    RelativePath = string.Empty;
                    AbsolutePath = string.Empty;
                    FileName = string.Empty;
                    Encoding = null;
                }

                disposed = true;
            }
        }
    }
}
