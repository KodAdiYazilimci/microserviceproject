using Infrastructure.Logging.Elastic.Configuration;

using Microsoft.Extensions.Configuration;

namespace Services.Logging.Exception.Configuration
{
    public class ExceptionLogElasticConfiguration : IElasticConfiguration
    {
        private readonly IConfiguration _configuration;

        public ExceptionLogElasticConfiguration(IConfiguration configuration)
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
                    .GetSection("ExceptionLogging")
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
                    .GetSection("ExceptionLogging")
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
                    .GetSection("ExceptionLogging")
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
                    .GetSection("ExceptionLogging")
                    .GetSection("ElasticConfiguration")["Index"] ?? string.Empty;
            }
        }
    }
}
