using Infrastructure.Security.Authentication.Abstract;

using Microsoft.Extensions.Configuration;

using System;
using System.Diagnostics;

namespace Infrastructure.Security.Authentication.Providers
{
    /// <summary>
    /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
    /// </summary>
    public class AppConfigCredentialProvider : ICredentialProvider, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Kullanıcı bilgilerini getiren configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Kullanıcı bilgilerini getiren configuration</param>
        public AppConfigCredentialProvider(IConfiguration configuration)
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
                    Convert.ToBoolean(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Authorization")
                        .GetSection("Credential")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Authorization")
                        .GetSection("Credential")["EnvironmentVariableNamePrefix"] + "_Email")
                    :
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Credential")["email"];
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
                    Convert.ToBoolean(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Authorization")
                        .GetSection("Credential")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                    ?
                    Environment.GetEnvironmentVariable(
                        _configuration
                        .GetSection("Configuration")
                        .GetSection("Authorization")
                        .GetSection("Credential")["EnvironmentVariableNamePrefix"] + "_Password")
                    :
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Credential")["password"];
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
