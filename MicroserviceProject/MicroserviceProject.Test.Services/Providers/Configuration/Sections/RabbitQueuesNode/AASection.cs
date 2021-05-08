﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

using System;
using System.Collections.Generic;

namespace MicroserviceProject.Test.Services.Providers.Configuration.Sections.RabbitQueuesNode
{
    public class AASection : BaseSection, IConfigurationSection
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
            return new AAChangeToken();
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
        public class AAChangeToken : IChangeToken
        {
            public bool HasChanged { get; }
            public bool ActiveChangeCallbacks { get; }
            public IDisposable RegisterChangeCallback(Action<object> callback, object state)
            {
                return new AADisposable();
            }
            public class AADisposable : IDisposable
            {
                public void Dispose() { }
            }
        }
    }
}