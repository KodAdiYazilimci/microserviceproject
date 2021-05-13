using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.WebSocketsNode
{
    public class WebSocketsSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }

        public IChangeToken GetReloadToken()
        {
            return new WebSocketsChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
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
