using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Configuration;
using MicroserviceProject.Model.Logging;

using System.IO;

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
        public void Log(TModel model)
        {
            string jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(model);

            DirectoryInfo directoryInfo = new DirectoryInfo(_fileConfiguration.Path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }


            System.IO.File.AppendAllText(_fileConfiguration.Path + "\\" + _fileConfiguration.FileName, jsonText, _fileConfiguration.Encoding);
        }
    }
}
