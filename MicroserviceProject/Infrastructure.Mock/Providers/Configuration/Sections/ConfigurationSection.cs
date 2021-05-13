

using Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.LocalizationNode;
using Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode;
using Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode;
using Infrastructure.Mock.Providers.Configuration.Sections.RoutingNode;
using Infrastructure.Mock.Providers.Configuration.Sections.WebSocketsNode;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections
{
    public class ConfigurationSection : BaseSection, IConfigurationSection
    {
        public AuthorizationSection AuthorizationSection { get; set; } = new AuthorizationSection();
        public LocalizationSection LocalizationSection { get; set; } = new LocalizationSection();
        public LoggingSection LoggingSection { get; set; } = new LoggingSection();
        public RabbitQueuesSection RabbitQueuesSection { get; set; } = new RabbitQueuesSection();
        public RoutingSection RoutingSection { get; set; } = new RoutingSection();
        public WebSocketsSection WebSocketsSection { get; set; } = new WebSocketsSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                AuthorizationSection,
                LocalizationSection,
                LoggingSection,
                RabbitQueuesSection,
                RoutingSection,
                WebSocketsSection
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
                case "WebSockets": return WebSocketsSection;
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
