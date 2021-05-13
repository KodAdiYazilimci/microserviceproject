using System;
using System.Text;

namespace Infrastructure.Logging.File.Configuration
{
    /// <summary>
    /// Dosyaya yazılacak logların yapılandırma arayüzü
    /// </summary>
    public interface IFileConfiguration
    {
        /// <summary>
        /// Yazılacak log dosyasının konumu
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// Yazılacak log dosyasının adı
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Yazılacak log dosyasının kodlaması
        /// </summary>
        Encoding Encoding { get; set; }
    }
}
