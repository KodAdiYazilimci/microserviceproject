using Infrastructure.ServiceDiscovery.Abstract;
using Infrastructure.ServiceDiscovery.Models;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.ServiceDiscovery.Providers
{
    public class AppConfigSolidServiceProvider : ISolidServiceProvider
    {
        private readonly  IConfiguration _configuration;

        public AppConfigSolidServiceProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SolidServiceModel GetSolidService()
        {
            return new SolidServiceModel()
            {
                Name = _configuration.GetSection("Configuration").GetSection("SolidService")["Name"].ToString(),
                RegisterAddress = _configuration.GetSection("Configuration").GetSection("SolidService")["RegisterAddress"].ToString(),
                DiscoverAddress = _configuration.GetSection("Configuration").GetSection("SolidService")["DiscoverAddress"].ToString()
            };
        }
    }
}
