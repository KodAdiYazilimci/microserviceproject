using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Configuration;
using MicroserviceProject.Model.Logging;

using System.IO;

namespace MicroserviceProject.Infrastructure.Logging.File.Loggers
{
    public class JsonFileLogger<TModel> : ILogger<TModel> where TModel : BaseLogModel, new()
    {
        private readonly IFileConfiguration _fileConfiguration;

        public JsonFileLogger(IFileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

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
