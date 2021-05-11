using Microsoft.Extensions.Configuration;

using System.Text;

namespace MicroserviceProject.Infrastructure.Security.Authentication.JWT.Providers
{
    /// <summary>
    /// JWT token için private key bilgisini sağlayan sınıf
    /// </summary>
    public class SecretProvider
    {
        /// <summary>
        /// Private key bilgisini getirecek configuration nesnesi
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// JWT token için private key bilgisini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Private key bilgisini getirecek configuration nesnesi</param>
        public SecretProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Private key bilgisini verir
        /// </summary>
        public string Key
        {
            get
            {
                return
                    configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("JWT")["JWTSecretKey"];
            }
        }

        /// <summary>
        /// Private key bilginin byte array olarak verir
        /// </summary>
        public byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Key);
            }
        }
    }
}
