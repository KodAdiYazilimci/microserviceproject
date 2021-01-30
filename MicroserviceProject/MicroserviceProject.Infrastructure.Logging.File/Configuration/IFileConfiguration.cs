using System.Text;

namespace MicroserviceProject.Infrastructure.Logging.File.Configuration
{
    public interface IFileConfiguration
    {
        string Path { get; set; }
        string FileName { get; set; }
        Encoding Encoding { get; set; }
    }
}
