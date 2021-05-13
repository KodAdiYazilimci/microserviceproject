using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode
{
    /// <summary>
    /// Logging düğümü sınıfı
    /// </summary>
    public class LoggingSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// RequestResponseLogging düğümü
        /// </summary>
        public RequestResponseLoggingSection RequestResponseLoggingSection { get; set; } = new RequestResponseLoggingSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                RequestResponseLoggingSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new LoggingChangeToken();
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
                case "RequestResponseLogging": return RequestResponseLoggingSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class LoggingChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new LoggingDisposable();
            }
            public class LoggingDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
