using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.AuthorizationNode
{
    /// <summary>
    /// Authorization düğümü sınıfı
    /// </summary>
    public class AuthorizationSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// Credentials düğümü
        /// </summary>
        public CredentialSection CredentialSection { get; set; } = new CredentialSection();

        /// <summary>
        /// DataSource düğümü
        /// </summary>
        public DataSourceSection DataSourceSection { get; set; } = new DataSourceSection();

        /// <summary>
        /// Endpoints düğümü
        /// </summary>
        public EndpointsSection EndpointsSection { get; set; } = new EndpointsSection();

        /// <summary>
        /// Jwt düğümü
        /// </summary>
        public JwtSection JwtSection { get; set; } = new JwtSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new AuthorizationChangeToken();
        }

        /// <summary>
        /// Alt düğümü verir
        /// </summary>
        /// <param name="key">Getirilecek alt düğümün adı</param>
        /// <returns></returns>
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

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
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
