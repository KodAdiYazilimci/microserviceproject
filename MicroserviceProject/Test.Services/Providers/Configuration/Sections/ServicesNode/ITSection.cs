using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.ServicesNode
{
    public class ITSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new ITChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class ITChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new ITDisposable();
            }
            public class ITDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}