using Infrastructure.ServiceDiscovery.Discoverer.Abstract;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Discoverer.Configuration
{
    public class AppConfigDiscoveryConfiguration : IDiscoveryConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfigDiscoveryConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public long ExpirationServiceInfo
        {
            get
            {
                return
                    Convert.ToInt64(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("ServiceDiscovery")
                    .GetSection("Discovery")["ExpirationServiceInfo"]);
            }
        }
    }
}
