using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Providers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Mock
{
    public static class AppConfigSolidServiceProviderProvider
    {
        public static ISolidServiceConfiguration GetSolidServiceConfiguration(IConfiguration configuration)
        {
            return new AppConfigSolidServiceConfiguration(configuration);
        }
    }
}
