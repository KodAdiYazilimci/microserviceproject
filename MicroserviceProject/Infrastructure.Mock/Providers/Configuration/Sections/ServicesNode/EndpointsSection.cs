using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.ServicesNode
{
    public class EndpointsSection : BaseSection, IConfigurationSection
    {
        public AASection AASection { get; set; } = new AASection();
        public AccountingSection AccountingSection { get; set; } = new AccountingSection();
        public BuyingSection BuyingSection { get; set; } = new BuyingSection();
        public FinanceSection FinanceSection { get; set; } = new FinanceSection();
        public HRSection HRSection { get; set; } = new HRSection();
        public ITSection ITSection { get; set; } = new ITSection();

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
        public IChangeToken GetReloadToken()
        {
            return new EndpointsChangeToken();
        }

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