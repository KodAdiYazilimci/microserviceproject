using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode
{
    public class LoggingSection : BaseSection, IConfigurationSection
    {
        public RequestResponseLoggingSection RequestResponseLoggingSection { get; set; } = new RequestResponseLoggingSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                RequestResponseLoggingSection
            };
        }

        public IChangeToken GetReloadToken()
        {
            return new LoggingChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "RequestResponseLogging": return RequestResponseLoggingSection;
                default:
                    return null;
            }
        }

        public class LoggingChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new LoggingDisposable();
            }
            public class LoggingDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
