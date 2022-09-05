using Microsoft.Extensions.Configuration;

using System.Diagnostics;
using System;
using System.Text;

namespace Infrastructure.Security.Authentication.JWT.Providers
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
                    Convert.ToBoolean(
                        configuration.GetSection("Configuration").GetSection("Authorization").GetSection("Jwt")["IsSensitiveData"] ?? false.ToString()) && !Debugger.IsAttached
                        ?
                        Environment.GetEnvironmentVariable(configuration.GetSection("Configuration").GetSection("Authorization").GetSection("Jwt")["EnvironmentVariableName"])
                        :
                        configuration.GetSection("Configuration").GetSection("Authorization").GetSection("Jwt")["JWTSecretKey"];
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
