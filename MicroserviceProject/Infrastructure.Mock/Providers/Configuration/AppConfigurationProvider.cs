
using Infrastructure.Mock.Providers.Configuration.Sections;
using Infrastructure.Mock.Providers.Configuration.Sections.PersistenceNode;
using Infrastructure.Mock.Providers.Configuration.Sections.ServicesNode;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration
{
    /// <summary>
    /// Yapılandırma ayarlarını taklit eden sınıf
    /// </summary>
    public class AppConfigurationProvider : IConfiguration
    {
        /// <summary>
        /// Ayarların değerleri
        /// </summary>
        private readonly Dictionary<string, string> Data = new Dictionary<string, string>();

        /// <summary>
        /// Bir ayar değeri verir
        /// </summary>
        /// <param name="key">Değeri getirilecek ayarın anahtarı</param>
        /// <returns></returns>
        public string this[string key] { get => Data[key]; set { Data[key] = value; } }

        /// <summary>
        /// Configuration düğümü
        /// </summary>
        public Sections.ConfigurationSection ConfigurationSection { get; set; } = new Sections.ConfigurationSection();

        /// <summary>
        /// Persistence düğümü
        /// </summary>
        public PersistenceSection PersistenceSection { get; set; } = new PersistenceSection();

        /// <summary>
        /// Services düğümü
        /// </summary>
        public ServicesSection ServicesSection { get; set; } = new ServicesSection();

        /// <summary>
        /// WebSockets düğümü
        /// </summary>
        public WebSocketsSection WebSocketsSection { get; set; } = new WebSocketsSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                ConfigurationSection,
                PersistenceSection,
                ServicesSection,
                WebSocketsSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new AppConfigurationChangeToken();
        }

        /// <summary>
        /// Alt düğümü verir
        /// </summary>
        /// <param name="key">Getirilecek alt düğümün adı</param>
        /// <returns></returns>
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "Configuration": return ConfigurationSection;
                case "Persistence": return PersistenceSection;
                case "Services": return ServicesSection;
                case "WebSockets": return WebSocketsSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class AppConfigurationChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new AppConfigurationDisposable();
            }
            public class AppConfigurationDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
