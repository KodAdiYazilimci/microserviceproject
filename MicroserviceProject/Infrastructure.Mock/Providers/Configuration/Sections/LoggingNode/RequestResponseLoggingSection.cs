using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.LoggingNode
{
    /// <summary>
    /// RequestResponseLogging düğümü sınıfı
    /// </summary>
    public class RequestResponseLoggingSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// FileConfiguration düğümü
        /// </summary>
        public FileConfigurationSection FileConfigurationSection { get; set; } = new FileConfigurationSection();

        /// <summary>
        /// RabbitConfiguration düğümü
        /// </summary>
        public RabbitConfigurationSection RabbitConfigurationSection { get; set; } = new RabbitConfigurationSection();

        /// <summary>
        /// DataBaseConfiguration düğümü
        /// </summary>
        public DataBaseConfigurationSection DataBaseConfigurationSection { get; set; } = new DataBaseConfigurationSection();

        /// <summary>
        /// MongoConfiguration düğümü
        /// </summary>
        public MongoConfigurationSection MongoConfigurationSection { get; set; } = new MongoConfigurationSection();

        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new RequestResponseLoggingChangeToken();
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
                case "FileConfiguration": return FileConfigurationSection;
                case "RabbitConfiguration": return RabbitConfigurationSection;
                case "DataBaseConfiguration": return DataBaseConfigurationSection;
                case "MongoConfiguration": return MongoConfigurationSection;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
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