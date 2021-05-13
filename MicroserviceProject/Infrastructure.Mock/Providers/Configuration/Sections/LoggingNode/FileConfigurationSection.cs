using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode
{
    public class FileConfigurationSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new FileConfigurationChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class FileConfigurationChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new FileConfigurationDisposable();
            }
            public class FileConfigurationDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}