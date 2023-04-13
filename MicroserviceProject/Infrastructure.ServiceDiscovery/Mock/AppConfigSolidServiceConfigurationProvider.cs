using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Configuration;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Mock
{
    public static class AppConfigSolidServiceConfigurationProvider
    {
        public static ISolidServiceConfiguration GetSolidServiceConfiguration(IConfiguration configuration)
        {
            return new AppConfigSolidServiceConfiguration(configuration);
        }
    }
}
