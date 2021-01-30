using MicroserviceProject.Infrastructure.Logging.File.Configuration;

using System.Text;

namespace MicroserviceProject.Services.Infrastructure.Logging.Logging.Configuration
{
    public class RequestResponseLogFileConfiguration : IFileConfiguration
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public Encoding Encoding { get; set; }
    }
}
