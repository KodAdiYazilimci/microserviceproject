using Infrastructure.ServiceDiscovery.Abstract;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Discoverer.Configuration
{
    public class AppConfigSolidServiceConfiguration : ISolidServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfigSolidServiceConfiguration(IConfiguration configuration)
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
                    .GetSection("SolidServiceConfiguration")["ExpirationServiceInfo"]);
            }
        }
    }
}
