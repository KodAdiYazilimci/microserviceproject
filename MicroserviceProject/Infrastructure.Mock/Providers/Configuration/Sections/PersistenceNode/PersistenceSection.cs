using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mock.Providers.Configuration.Sections.PersistenceNode
{
    public class PersistenceSection : BaseSection, IConfigurationSection
    {
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }
        public IChangeToken GetReloadToken()
        {
            return new PersistenceChangeToken();
        }
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }
        public class PersistenceChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }

            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new PersistenceDisposable();
            }
            public class PersistenceDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
