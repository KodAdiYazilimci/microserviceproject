using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.RoutingNode
{
    public class RoutingSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }

        public IChangeToken GetReloadToken()
        {
            return new RoutingChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class RoutingChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new RoutingDisposable();
            }
            public class RoutingDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
