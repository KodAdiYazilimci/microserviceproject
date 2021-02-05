using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Infrastructure.Communication.Moderator.Providers
{
    /// <summary>
    /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
    /// </summary>
    public class CredentialProvider
    {
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
    }
}
