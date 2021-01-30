﻿using MicroserviceProject.Infrastructure.Logging.Abstraction;
using MicroserviceProject.Infrastructure.Logging.File.Configuration;
using MicroserviceProject.Model.Logging;

using System.IO;

namespace MicroserviceProject.Infrastructure.Logging.File.Loggers
{
    /// <summary>
    /// Düz metin formatta log yazan sınıf
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class TextFileLogger<TModel> : ILogger<TModel> where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Yazılacak log dosyasının yapılandırması
        /// </summary>
        private readonly IFileConfiguration _fileConfiguration;

        /// <summary>
        /// Düz metin formatta log yazan sınıf
        /// </summary>
        /// <param name="fileConfiguration">Yazılacak log dosyasının yapılandırması</param>
        public TextFileLogger(IFileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

        /// <summary>
        /// Düz metin log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        public void Log(TModel model)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(_fileConfiguration.Path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            System.IO.File.AppendAllText(_fileConfiguration.Path + "\\" + _fileConfiguration.FileName, model.ToString(), _fileConfiguration.Encoding);
        }
    }
}