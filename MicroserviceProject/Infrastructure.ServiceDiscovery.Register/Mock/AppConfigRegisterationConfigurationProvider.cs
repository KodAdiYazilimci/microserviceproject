using Infrastructure.ServiceDiscovery.Register.Abstract;
using Infrastructure.ServiceDiscovery.Register.Configuration;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Register.Mock
{
    public static class AppConfigRegisterationConfigurationProvider
    {
        public static IRegisterationConfiguration GetRegisterationConfiguration(IConfiguration configuration)
        {
            return new AppConfigRegisterationConfiguration(configuration);
        }
    }
}
