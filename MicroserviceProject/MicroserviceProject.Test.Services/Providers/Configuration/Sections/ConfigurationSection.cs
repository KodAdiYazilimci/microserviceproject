using MicroserviceProject.Test.Services.Providers.Configuration.Sections.AuthorizationNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.LocalizationNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.LoggingNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.RabbitQueuesNode;
using MicroserviceProject.Test.Services.Providers.Configuration.Sections.RoutingNode;


using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections
{
    public class ConfigurationSection : BaseSection, IConfigurationSection
    {
        public AuthorizationSection AuthorizationSection { get; set; } = new AuthorizationSection();
        public LocalizationSection LocalizationSection { get; set; } = new LocalizationSection();
        public LoggingSection LoggingSection { get; set; } = new LoggingSection();
        public RabbitQueuesSection RabbitQueuesSection { get; set; } = new RabbitQueuesSection();
        public RoutingSection RoutingSection { get; set; } = new RoutingSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                AuthorizationSection,
                LocalizationSection,
                LoggingSection,
                RabbitQueuesSection,
                RoutingSection
            };
        }
        public IChangeToken GetReloadToken()
        {
            return new ConfigurationChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "Authorization": return AuthorizationSection;
                case "Localization": return LocalizationSection;
                case "Logging": return LoggingSection;
                case "RabbitQueues": return RabbitQueuesSection;
                case "Routing": return RoutingSection;
                default:
                    return null;
            }
        }
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
