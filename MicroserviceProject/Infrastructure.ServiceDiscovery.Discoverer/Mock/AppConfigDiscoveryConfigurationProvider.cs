using Infrastructure.ServiceDiscovery.Discoverer.Abstract;
using Infrastructure.ServiceDiscovery.Discoverer.Configuration;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Discoverer.Mock
{
    public static class AppConfigDiscoveryConfigurationProvider
    {
        public static IDiscoveryConfiguration GetDiscoveryConfiguration(IConfiguration configuration)
        {
            return new AppConfigDiscoveryConfiguration(configuration);
        }
    }
}
