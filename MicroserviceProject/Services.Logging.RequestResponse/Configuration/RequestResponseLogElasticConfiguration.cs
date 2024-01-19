using Infrastructure.Logging.Elastic.Configuration;

using Microsoft.Extensions.Configuration;

namespace Services.Logging.RequestResponse.Configuration
{
    public class RequestResponseLogElasticConfiguration: IElasticConfiguration
    {
        private readonly IConfiguration _configuration;

        public RequestResponseLogElasticConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Host
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RequestResponseLogging")
                    .GetSection("ElasticConfiguration")["Host"] ?? string.Empty;
            }
        }
        public string UserName
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RequestResponseLogging")
                    .GetSection("ElasticConfiguration")["UserName"] ?? string.Empty;
            }
        }
        public string Password
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RequestResponseLogging")
                    .GetSection("ElasticConfiguration")["Password"] ?? string.Empty;
            }
        }
        public string Index
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Logging")
                    .GetSection("RequestResponseLogging")
                    .GetSection("ElasticConfiguration")["Index"] ?? string.Empty;
            }
        }
    }
}
