using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.AuthorizationNode
{
    public class JwtSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new JwtSectionChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class JwtSectionChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new JwtSectionDisposable();
            }
            public class JwtSectionDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
