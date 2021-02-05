using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Infrastructure.Communication.Moderator.Providers
{
    /// <summary>
    /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
    /// </summary>
    public class RouteProvider
    {
        /// <summary>
        /// Endpoint isimlerini getiren configuration
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Servis rotalarına ait endpoint isimlerini sağlayan sınıf
        /// </summary>
        /// <param name="configuration">Endpoint isimlerini getiren configuration</param>
        public RouteProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Kullanıcı bilgilerine göre token getirir
        /// </summary>
        public string Auth_GetToken
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Endpoints")
                    .GetSection("GetToken").Value;
            }
        }

        /// <summary>
        /// Token bilgisine göre kullanıcı bilgisini getirir
        /// </summary>
        public string Auth_GetUser
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("Endpoints")
                    .GetSection("GetUser").Value;
            }
        }

        public string SampleDataProvider_GetData
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("SampleDataProvider")
                    .GetSection("GetData").Value;
            }
        }

        public string SampleDataProvider_PostData
        {
            get
            {
                return
                    _configuration
                    .GetSection("Services")
                    .GetSection("Endpoints")
                    .GetSection("SampleDataProvider")
                    .GetSection("PostData").Value;
            }
        }
    }
}
