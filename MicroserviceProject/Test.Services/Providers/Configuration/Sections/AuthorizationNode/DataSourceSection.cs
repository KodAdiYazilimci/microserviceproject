using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.AuthorizationNode
{
    public class DataSourceSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new DataSourceSectionChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class DataSourceSectionChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new DataSourceSectionDisposable();
            }
            public class DataSourceSectionDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
