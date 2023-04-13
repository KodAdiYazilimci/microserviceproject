using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Providers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Mock
{
    public static class AppConfigSolidServiceProviderProvider
    {
        public static ISolidServiceProvider GetSolidServiceConfiguration(IConfiguration configuration)
        {
            return new AppConfigSolidServiceProvider(configuration);
        }
    }
}
