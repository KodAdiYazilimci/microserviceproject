using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.LoggingNode
{
    public class RabbitConfigurationSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new RabbitConfigurationChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class RabbitConfigurationChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new RabbitConfigurationDisposable();
            }
            public class RabbitConfigurationDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}