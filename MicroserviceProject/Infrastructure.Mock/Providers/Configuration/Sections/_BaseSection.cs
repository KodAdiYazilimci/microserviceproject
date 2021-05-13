using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections
{
    /// <summary>
    /// Düğümlerin uygulanacağı temel sınıf
    /// </summary>
    public class BaseSection
    {
        /// <summary>
        /// Ayarların değerleri
        /// </summary>
        protected readonly Dictionary<string, string> Data = new Dictionary<string, string>();

        /// <summary>
        /// Bir ayar değeri verir
        /// </summary>
        /// <param name="key">Değeri getirilecek ayarın anahtarı</param>
        /// <returns></returns>
        public string this[string key] { get => Data[key]; set { Data[key] = value; } }

        /// <summary>
        /// Ayarın anahtarı
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Ayarın yolu
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Ayarın değeri
        /// </summary>
        public string Value { get; set; }
    }
}
