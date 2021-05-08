
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.PersistenceNode;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration
{
    public class AppConfigurationProvider : IConfiguration
    {
        private readonly Dictionary<string, string> Data = new Dictionary<string, string>();
        public string this[string key] { get => Data[key]; set { Data[key] = value; } }

        public Sections.ConfigurationSection ConfigurationSection { get; set; } = new Sections.ConfigurationSection();
        public PersistenceSection PersistenceSection { get; set; } = new PersistenceSection();
        public Sections.ServicesNode.ServicesSection ServicesSection { get; set; } = new Sections.ServicesNode.ServicesSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                ConfigurationSection,
                PersistenceSection,
                ServicesSection
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
