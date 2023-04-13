using Infrastructure.ServiceDiscovery.Abstract;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Configuration
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
                    .GetSection("ServiceDiscovery")
                    .GetSection("Discovery")["ExpirationServiceInfo"]);
            }
        }

        public bool OverrideDnsName
        {
            get
            {
                return
                    Convert.ToBoolean(
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("ServiceDiscovery")
                    .GetSection("Registeration")["OverrideDnsName"]);
            }
        }
        public string OverridenDnsName
        {
            get
            {
                return
                   _configuration
                   .GetSection("Configuration")
                   .GetSection("ServiceDiscovery")
                   .GetSection("Registeration")["OverridenDnsName"].ToString();
            }
        }
    }
}
