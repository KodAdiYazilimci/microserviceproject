using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Infrastructure.Security.Authentication.JWT.Providers
{
    /// <summary>
    /// JWT token için yayımcı bilgisini sağlayan sınıf
    /// </summary>
    public class IssuerProvider
    {
        /// <summary>
        /// Yayımcı bilgilerinin getirilecek configuration nesnesi
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// JWT token için yayımcı bilgisini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Yayımcı bilgilerinin getirilecek configuration nesnesi</param>
        public IssuerProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Yayımcı bilgisini verir
        /// </summary>
        public string Issuer
        {
            get
            {
                return
                    configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Jwt")["Issuer"];
            }
        }

        /// <summary>
        /// Yayımcı bilgisini verir
        /// </summary>
        public string Audience
        {
            get
            {
                return
                    configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Jwt")["Audience"];
            }
        }
    }
}
