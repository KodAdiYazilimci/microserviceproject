using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace Infrastructure.Mock.Providers.Configuration.Sections.CachingNode
{
    /// <summary>
    /// Redis düğümü sınıfı
    /// </summary>
    public class RedisSection : BaseSection, IConfigurationSection
    {
        /// <summary>
        /// Alt düğümleri verir
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return null;
        }

        /// <summary>
        /// Yenileme tokenı verir
        /// </summary>
        /// <returns></returns>
        public IChangeToken GetReloadToken()
        {
            return new RedisChangeToken();
        }

        /// <summary>
        /// Alt düğümü verir
        /// </summary>
        /// <param name="key">Getirilecek alt düğümün adı</param>
        /// <returns></returns>
        public IConfigurationSection GetSection(string key)
        {
            return this;
        }

        /// <summary>
        /// Değişim token sınıfı
        /// </summary>
        public class RedisChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }

            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new RedisDisposable();
            }
            public class RedisDisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}
