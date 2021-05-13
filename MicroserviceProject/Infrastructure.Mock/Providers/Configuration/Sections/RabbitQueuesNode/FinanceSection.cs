using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    /// <summary>
    /// Finance düğümü sınıfı
    /// </summary>
    public class FinanceSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// QueueNames düğümü
        /// </summary>
        public QueueNamesSection QueueNamesSection { get; set; } = new QueueNamesSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                QueueNamesSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new FinanceChangeToken();
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
                case "QueueNames": return QueueNamesSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class FinanceChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new FinanceDisposable();
            }
            public class FinanceDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}