using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.File.Configuration;
using Infrastructure.Logging.Model;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Logging.File.Loggers
{
    /// <summary>
    /// Json formatta log yazma sınıfı
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class JsonFileLogger<TModel> : ILogger<TModel>, IDisposable where TModel : BaseLogModel, new()
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Yazılacak log dosyasının yapılandırması
        /// </summary>
        private IFileConfiguration _fileConfiguration;

        /// <summary>
        /// Json formatta log yazma sınıfı
        /// </summary>
        /// <param name="fileConfiguration">Yazılacak log dosyasının yapılandırması</param>
        public JsonFileLogger(IFileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

        private static ReaderWriterLockSlim ReaderWriterLockSlim => new ReaderWriterLockSlim();

        /// <summary>
        /// Json formatta log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        public async Task LogAsync(TModel model, CancellationTokenSource cancellationTokenSource)
        {
            ReaderWriterLockSlim.EnterWriteLock();

            try
            {
                StringBuilder sbJsonText = new StringBuilder(JsonConvert.SerializeObject(model));

                if (sbJsonText.Length > 0)
                {
                    sbJsonText.Append("\r\n");
                }

                if (!string.IsNullOrEmpty(_fileConfiguration.RelativePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + "/" + _fileConfiguration.RelativePath);

                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }

                    await System.IO.File.AppendAllTextAsync(
                        path: Environment.CurrentDirectory + "/" + _fileConfiguration.RelativePath + "/" + _fileConfiguration.FileName,
                        contents: sbJsonText.ToString(),
                        encoding: _fileConfiguration.Encoding,
                        cancellationToken: cancellationTokenSource.Token);
                }
                else if (!string.IsNullOrEmpty(_fileConfiguration.AbsolutePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_fileConfiguration.AbsolutePath);

                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }

                    await System.IO.File.AppendAllTextAsync(
                        path: _fileConfiguration.AbsolutePath + "\\" + _fileConfiguration.FileName,
                        contents: sbJsonText.ToString(),
                        encoding: _fileConfiguration.Encoding,
                        cancellationToken: cancellationTokenSource.Token);
                }
            }
            finally
            {
                ReaderWriterLockSlim.ExitWriteLock();
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _fileConfiguration = null;
                }

                disposed = true;
            }
        }
    }
}
