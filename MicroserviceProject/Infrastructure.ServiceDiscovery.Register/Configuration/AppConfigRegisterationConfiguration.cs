using Infrastructure.ServiceDiscovery.Register.Abstract;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Register.Configuration
{
    public class AppConfigRegisterationConfiguration : IRegisterationConfiguration
    {
        private readonly IConfiguration _configuration;

        public AppConfigRegisterationConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
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

        public string ServiceName
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("ServiceDiscovery")
                    .GetSection("Registeration")["ServiceName"].ToString();
            }
        }
        public int Port
        {
            get
            {
                return
                   Convert.ToInt32(
                   _configuration
                   .GetSection("Configuration")
                   .GetSection("ServiceDiscovery")
                   .GetSection("Registeration")["Port"]);
            }
        }
        public string Protocol
        {
            get
            {
                return
                   _configuration
                   .GetSection("Configuration")
                   .GetSection("ServiceDiscovery")
                   .GetSection("Registeration")["Protocol"].ToString();
            }
        }
    }
}
