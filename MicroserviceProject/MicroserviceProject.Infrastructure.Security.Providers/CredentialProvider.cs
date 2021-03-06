using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Infrastructure.Security.Providers
{
    /// <summary>
    /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
    /// </summary>
    public class CredentialProvider : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        protected bool Disposed = false;

        /// <summary>
        /// Kullanıcı bilgilerini getiren configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Kullanıcı bilgilerini getiren configuration</param>
        public CredentialProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Servise ait e-posta adresi
        /// </summary>
        public string GetEmail
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Credential")
                    .GetSection("email").Value;
            }
        }

        /// <summary>
        /// Servise ait parola
        /// </summary>
        public string GetPassword
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Credential")
                    .GetSection("password").Value;
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
                if (!Disposed)
                {

                }

                Disposed = true;
            }
        }
    }
}
