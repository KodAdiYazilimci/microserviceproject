using Infrastructure.Logging.Abstraction;
using Infrastructure.Logging.File.Configuration;
using Infrastructure.Logging.Model;

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Logging.File.Loggers
{
    /// <summary>
    /// Düz metin formatta log yazan sınıf
    /// </summary>
    /// <typeparam name="TModel">Yazılacak logun tipi</typeparam>
    public class TextFileLogger<TModel> : ILogger<TModel>, IDisposable where TModel : BaseLogModel, new()
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
        /// Düz metin formatta log yazan sınıf
        /// </summary>
        /// <param name="fileConfiguration">Yazılacak log dosyasının yapılandırması</param>
        public TextFileLogger(IFileConfiguration fileConfiguration)
        {
            _fileConfiguration = fileConfiguration;
        }

        private static ReaderWriterLockSlim ReaderWriterLockSlim => new ReaderWriterLockSlim();

        /// <summary>
        /// Düz metin log yazar
        /// </summary>
        /// <param name="model">Yazılacak logun modeli</param>
        public async Task LogAsync(TModel model, CancellationTokenSource cancellationTokenSource)
        {
            ReaderWriterLockSlim.EnterWriteLock();

            try
            {
                if (!string.IsNullOrEmpty(_fileConfiguration.RelativePath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(Environment.CurrentDirectory + "/" + _fileConfiguration.RelativePath);

                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }

                    await System.IO.File.AppendAllTextAsync(
                        path: Environment.CurrentDirectory + "/" + _fileConfiguration.RelativePath + "/" + _fileConfiguration.FileName,
                        contents: model.ToString(),
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
                        contents: model.ToString(),
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
