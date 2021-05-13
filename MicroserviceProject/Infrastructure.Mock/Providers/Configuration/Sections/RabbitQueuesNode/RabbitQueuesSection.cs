using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    /// <summary>
    /// RabbitQueues düğümü sınıfı
    /// </summary>
    public class RabbitQueuesSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// Services düğümü
        /// </summary>
        public ServicesSection ServicesSection { get; set; } = new ServicesSection();

        /// <summary>
        /// Host düğümü
        /// </summary>
        public HostSection HostSection { get; set; } = new HostSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                ServicesSection,
                HostSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new RabbitQueuesChangeToken();
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
                case "Services": return ServicesSection;
                case "Host": return HostSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class RabbitQueuesChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new RabbitQueuesDisposable();
            }
            public class RabbitQueuesDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
