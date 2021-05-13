using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.ServicesNode
{
    /// <summary>
    /// Endpoint düğümü sınıfı
    /// </summary>
    public class EndpointsSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// AA düğümü
        /// </summary>
        public AASection AASection { get; set; } = new AASection();

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
        /// HR düğümü
        /// </summary>
        public HRSection HRSection { get; set; } = new HRSection();

        /// <summary>
        /// IT düğümü
        /// </summary>
        public ITSection ITSection { get; set; } = new ITSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                AASection,
                AccountingSection,
                BuyingSection,
                FinanceSection,
                HRSection,
                ITSection
            };
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new EndpointsChangeToken();
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
                case "Accounting": return AccountingSection;
                case "Buying": return BuyingSection;
                case "Finance": return FinanceSection;
                case "HR": return HRSection;
                case "IT": return ITSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class EndpointsChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new EndpointsDisposable();
            }
            public class EndpointsDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}