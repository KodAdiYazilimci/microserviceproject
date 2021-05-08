using MicroserviceProject.Test.Services.Providers.Configuration.Sections.AuthorizationNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.LocalizationNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.LoggingNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.PersistenceNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.RabbitQueuesNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.RoutingNode;

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

        public AuthorizationSection AuthorizationSection { get; set; } = new AuthorizationSection();
        public LocalizationSection LocalizationSection { get; set; } = new LocalizationSection();
        public LoggingSection LoggingSection { get; set; } = new LoggingSection();
        public PersistenceSection PersistenceSection { get; set; } = new PersistenceSection();
        public RabbitQueuesSection RabbitQueuesSection { get; set; } = new RabbitQueuesSection();
        public RoutingSection RoutingSection { get; set; } = new RoutingSection();
        public Sections.ServicesNode.ServicesSection ServicesSection { get; set; } = new Sections.ServicesNode.ServicesSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                AuthorizationSection,
                LocalizationSection,
                LoggingSection,
                PersistenceSection,
                RabbitQueuesSection,
                RoutingSection,
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
                case "Authorization": return AuthorizationSection;
                case "Localization": return LocalizationSection;
                case "Logging": return LoggingSection;
                case "Persistence":return PersistenceSection;
                case "RabbitQueues": return RabbitQueuesSection;
                case "Routing": return RoutingSection;
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
