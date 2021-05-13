using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    public class HostSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new HostSectionChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class HostSectionChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new HostSectionDisposable();
            }
            public class HostSectionDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}