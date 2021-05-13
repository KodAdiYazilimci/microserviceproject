using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections
{
    public class WebSocketsSection : BaseSection, IConfigurationSection
    {
        public EndpointsSection EndpointsSection { get; set; } = new EndpointsSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                EndpointsSection
            };
        }

        public IChangeToken GetReloadToken()
        {
            return new WebSocketsChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "Endpoints": return EndpointsSection;
                default:
                    return null;
            }
        }
        public class WebSocketsChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new WebSocketsDisposable();
            }
            public class WebSocketsDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
