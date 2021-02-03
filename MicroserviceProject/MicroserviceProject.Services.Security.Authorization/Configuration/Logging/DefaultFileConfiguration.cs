using MicroserviceProject.Infrastructure.Logging.File.Configuration;

using Microsoft.Extensions.Configuration;

using System.Text;

namespace MicroserviceProject.Services.Security.Authorization.Configuration.Logging
{
    public class DefaultFileConfiguration : IFileConfiguration
    {
        public DefaultFileConfiguration(IConfiguration configuration)
        {
            Path = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("Path").Value;

            FileName = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("FileName").Value;

            string encodingCode = configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("FileConfiguration")
                .GetSection("Encoding").Value;

            Encoding = Encoding.GetEncoding(encodingCode);
        }

        public string Path { get; set; }
        public string FileName { get; set; }
        public Encoding Encoding { get; set; }
    }
}
