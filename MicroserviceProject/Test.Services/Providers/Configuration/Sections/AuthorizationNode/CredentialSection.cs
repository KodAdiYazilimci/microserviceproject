using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.AuthorizationNode
{
    public class CredentialSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new CredentialChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class CredentialChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new CredentialDisposable();
            }
            public class CredentialDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
