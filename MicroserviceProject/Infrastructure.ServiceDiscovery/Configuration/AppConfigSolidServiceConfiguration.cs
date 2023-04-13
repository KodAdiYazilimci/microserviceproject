using Infrastructure.ServiceDiscovery.Abstract;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Providers
{
    public class AppConfigSolidServiceConfiguration : ISolidServiceConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfigSolidServiceConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Name
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("ServiceDiscovery")
                    .GetSection("SolidService")["Name"].ToString();
            }
        }
        public string RegisterAddress
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("ServiceDiscovery")
                    .GetSection("SolidService")["RegisterAddress"].ToString();
            }
        }
        public string DiscoverAddress
        {
            get
            {
                return
                     _configuration
                     .GetSection("Configuration")
                     .GetSection("ServiceDiscovery")
                     .GetSection("SolidService")["DiscoverAddress"].ToString();
            }
        }
    }
}
