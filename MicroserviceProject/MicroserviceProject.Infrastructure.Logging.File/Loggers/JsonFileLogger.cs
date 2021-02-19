using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Configuration;
using MicroserviceProject.Infrastructure.Logging.Model;

using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Infrastructure.Logging.File.Loggers
{
    /// <summary>
    /// Json formatta log yazma sınıfı
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class JsonFileLogger<TModel> : ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Yazılacak log dosyasının yapılandırması
        /// </summary>
        private readonly IFileConfiguration _fileConfiguration;

        /// <summary>
        /// Json formatta log yazma sınıfı
        /// </summary>
        /// <param name="fileConfiguration">Yazılacak log dosyasının yapılandırması</param>
        public JsonFileLogger(IFileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

        /// <summary>
        /// Json formatta log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        public async Task LogAsync(TModel model, CancellationToken cancellationToken)
        {
            StringBuilder sbJsonText = new StringBuilder(Newtonsoft.Json.JsonConvert.SerializeObject(model));

            if (sbJsonText.Length > 0)
            {
                sbJsonText.Append("\r\n");
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(_fileConfiguration.Path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            await System.IO.File.AppendAllTextAsync(
                path: _fileConfiguration.Path + "\\" + _fileConfiguration.FileName,
                contents: sbJsonText.ToString(),
                encoding: _fileConfiguration.Encoding,
                cancellationToken: cancellationToken);
        }
    }
}
