

using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.LocalizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode;
using Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections
{
    /// <summary>
    /// Configuration düğümü sınıfı
    /// </summary>
    public class ConfigurationSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// Authorization düğümü
        /// </summary>
        public AuthorizationSection AuthorizationSection { get; set; } = new AuthorizationSection();

        /// <summary>
        /// Localization düğümü
        /// </summary>
        public LocalizationSection LocalizationSection { get; set; } = new LocalizationSection();

        /// <summary>
        /// Logging düğümü
        /// </summary>
        public LoggingSection LoggingSection { get; set; } = new LoggingSection();

        /// <summary>
        /// RabbitQueues düğümü
        /// </summary>
        public RabbitQueuesSection RabbitQueuesSection { get; set; } = new RabbitQueuesSection();

        /// <summary>
        /// WebSockets düğümğ
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
                AuthorizationSection,
                LocalizationSection,
                LoggingSection,
                RabbitQueuesSection,
                WebSocketsSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new ConfigurationChangeToken();
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
                case "Authorization": return AuthorizationSection;
                case "Localization": return LocalizationSection;
                case "Logging": return LoggingSection;
                case "RabbitQueues": return RabbitQueuesSection;
                case "WebSockets": return WebSocketsSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class ConfigurationChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new ConfigurationDisposable();
            }
            public class ConfigurationDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
