using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode
{
    public class RequestResponseLoggingSection : BaseSection, IConfigurationSection
    {
        public FileConfigurationSection FileConfigurationSection { get; set; } = new FileConfigurationSection();
        public RabbitConfigurationSection RabbitConfigurationSection { get; set; } = new RabbitConfigurationSection();
        public DataBaseConfigurationSection DataBaseConfigurationSection { get; set; } = new DataBaseConfigurationSection();
        public MongoConfigurationSection MongoConfigurationSection { get; set; } = new MongoConfigurationSection();

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>()
            {
                FileConfigurationSection,
                RabbitConfigurationSection,
                DataBaseConfigurationSection,
                MongoConfigurationSection
            };
        }

        public IChangeToken GetReloadToken()
        {
            return new RequestResponseLoggingChangeToken();
        }

        public IConfigurationSection GetSection(string key)
        {
            switch (key)
            {
                case "FileConfiguration": return FileConfigurationSection;
                case "RabbitConfiguration": return RabbitConfigurationSection;
                case "DataBaseConfiguration": return DataBaseConfigurationSection;
                case "MongoConfiguration": return MongoConfigurationSection;
                default:
                    return null;
            }
        }

        public class RequestResponseLoggingChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new RequestResponseLoggingDisposable();
            }
            public class RequestResponseLoggingDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}