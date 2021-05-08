using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.ServicesNode
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
            throw new System.NotImplementedException();
        }
        public IChangeToken GetReloadToken()
        {
            return new EndpointsChangeToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new System.NotImplementedException();
        }
        public class EndpointsChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                throw new NotImplementedException();
            }
            public class EndpointsDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}