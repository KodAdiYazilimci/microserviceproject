using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections
{
    /// <summary>
    /// WebSockets düğümü sınıfı
    /// </summary>
    public class WebSocketsSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// Endpoints düğümü
        /// </summary>
        public EndpointsSection EndpointsSection { get; set; } = new EndpointsSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                EndpointsSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new WebSocketsChangeToken();
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
                case "Endpoints": return EndpointsSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class WebSocketsChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new WebSocketsDisposable();
            }
            public class WebSocketsDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
