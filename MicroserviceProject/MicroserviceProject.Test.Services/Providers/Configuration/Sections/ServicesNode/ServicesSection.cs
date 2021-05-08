using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.ServicesNode
{
    public class ServicesSection : BaseSection, IConfigurationSection
    {
        public EndpointsSection EndpointsSection { get; set; } = new EndpointsSection();
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                EndpointsSection
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
                case "Endpoints": return EndpointsSection;
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
