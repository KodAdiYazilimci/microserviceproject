using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode
{
    public class AuthorizationSection : BaseSection, IConfigurationSection
    {
        public CredentialSection CredentialSection { get; set; } = new CredentialSection();
        public DataSourceSection DataSourceSection { get; set; } = new DataSourceSection();
        public EndpointsSection EndpointsSection { get; set; } = new EndpointsSection();
        public JwtSection JwtSection { get; set; } = new JwtSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                CredentialSection,
                DataSourceSection,
                EndpointsSection,
                JwtSection
            };
        }
        public IChangeToken GetReloadToken()
        {
            return new AuthorizationChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "Credential": return CredentialSection;
                case "Endpoints": return EndpointsSection;
                case "DataSource": return DataSourceSection;
                case "Jwt": return JwtSection;
                default:
                    return null;
            }
        }
        public class AuthorizationChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new AuthorizationDisposable();
            }
            public class AuthorizationDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
