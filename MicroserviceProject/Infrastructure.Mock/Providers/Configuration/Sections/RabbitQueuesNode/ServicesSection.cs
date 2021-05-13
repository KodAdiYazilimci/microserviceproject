using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    public class ServicesSection : BaseSection, IConfigurationSection
    {
        public AASection AASection { get; set; } = new AASection();
        public ITSection ITSection { get; set; } = new ITSection();
        public AccountingSection AccountingSection { get; set; } = new AccountingSection();
        public BuyingSection BuyingSection { get; set; } = new BuyingSection();
        public FinanceSection FinanceSection { get; set; } = new FinanceSection();
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
        public IChangeToken GetReloadToken()
        {
            return new ServicesChangeToken();
        }
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