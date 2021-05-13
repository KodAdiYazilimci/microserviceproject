
using Infrastructure.Mock.Providers.Configuration.Sections;
using Infrastructure.Mock.Providers.Configuration.Sections.PersistenceNode;
using Infrastructure.Mock.Providers.Configuration.Sections.ServicesNode;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration
{
    public class AppConfigurationProvider : IConfiguration
    {
        private readonly Dictionary<string, string> Data = new Dictionary<string, string>();
        public string this[string key] { get => Data[key]; set { Data[key] = value; } }

        public ConfigurationSection ConfigurationSection { get; set; } = new ConfigurationSection();
        public PersistenceSection PersistenceSection { get; set; } = new PersistenceSection();
        public ServicesSection ServicesSection { get; set; } = new ServicesSection();
        public WebSocketsSection WebSocketsSection { get; set; } = new WebSocketsSection();

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
        public IChangeToken GetReloadToken()
        {
            return new AppConfigurationChangeToken();
        }
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
