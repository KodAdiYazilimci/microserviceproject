using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    /// <summary>
    /// Services düğümü sınıfı
    /// </summary>
    public class ServicesSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// AA düğümü
        /// </summary>
        public AASection AASection { get; set; } = new AASection();

        /// <summary>
        /// IT düğümü
        /// </summary>
        public ITSection ITSection { get; set; } = new ITSection();

        /// <summary>
        /// Accounting düğümü
        /// </summary>
        public AccountingSection AccountingSection { get; set; } = new AccountingSection();

        /// <summary>
        /// Buying düğümü
        /// </summary>
        public BuyingSection BuyingSection { get; set; } = new BuyingSection();

        /// <summary>
        /// Finance düğümü
        /// </summary>
        public FinanceSection FinanceSection { get; set; } = new FinanceSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                AASection,
                ITSection,
                AccountingSection,
                BuyingSection,
                FinanceSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new ServicesChangeToken();
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
                case "AA": return AASection;
                case "IT": return ITSection;
                case "Accounting": return AccountingSection;
                case "Buying": return BuyingSection;
                case "Finance": return FinanceSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class ServicesChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new ServicesDisposable();
            }
            public class ServicesDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}