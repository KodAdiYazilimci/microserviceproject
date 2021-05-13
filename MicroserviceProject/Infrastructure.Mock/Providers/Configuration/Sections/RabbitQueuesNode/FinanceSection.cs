using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.RabbitQueuesNode
{
    public class FinanceSection : BaseSection, IConfigurationSection
    {
        public QueueNamesSection QueueNamesSection { get; set; } = new QueueNamesSection();
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                QueueNamesSection
            };
        }
        public IChangeToken GetReloadToken()
        {
            return new FinanceChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "QueueNames": return QueueNamesSection;
                default:
                    return null;
            }
        }
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