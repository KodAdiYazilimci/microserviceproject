using Microsoft.Extensions.Configuration;

using System;

namespace Infrastructure.Sockets.Providers
{
    /// <summary>
    /// Web soketlere ait isimleri sağlayan sınıf
    /// </summary>
    public class SocketNameProvider : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Endpoint isimlerini getiren configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Web soketlere ait isimleri sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Endpoint isimlerini getiren configuration</param>
        public SocketNameProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Token mesajlarını getirir
        /// </summary>
        public string Security_TokensHub_GetTokenMessages
        {
            get
            {
                return
                    _configuration
                    .GetSection("WebSockets")
                    .GetSection("Endpoints")["TokensHub.GetTokenMessages"];
            }
        }

        /// <summary>
        /// Hata mesajlarını getirir
        /// </summary>
        public string Reliability_ErrorHub_GetErrorMessages
        {
            get
            {
                return
                    _configuration
                    .GetSection("WebSockets")
                    .GetSection("Endpoints")["ErrorHub.GetErrorMessages"];
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

                }

                disposed = true;
            }
        }
    }
}
