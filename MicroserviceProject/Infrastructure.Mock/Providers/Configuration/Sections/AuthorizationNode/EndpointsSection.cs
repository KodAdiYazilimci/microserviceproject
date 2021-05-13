using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode
{
    public class EndpointsSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new EndpointsChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class EndpointsChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new EndpointsDisposable();
            }
            public class EndpointsDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
