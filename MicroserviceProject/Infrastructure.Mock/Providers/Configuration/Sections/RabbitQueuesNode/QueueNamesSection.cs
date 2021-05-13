using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    public class QueueNamesSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new QueueNamesChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class QueueNamesChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new QueueNamesDisposable();
            }
            public class QueueNamesDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}