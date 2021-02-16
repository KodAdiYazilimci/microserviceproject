using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Repositories
{
    /// <summary>
    /// Repository sınıfların temel sınıfı
    /// </summary>
    public abstract class BaseRepository
    {
        /// <summary>
        /// Veritabanı bağlantı cümlesini getirecek configuration nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Repository sınıfların temel sınıfı
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini getirecek configuration nesnesi</param>
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        /// Yetki altyapısı için kullanılacak veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <returns></returns>
        protected string AuthorizationConnectionString
        {
            get
            {
                return
                    _configuration
                    .GetSection("Configuration")
                    .GetSection("Authorization")
                    .GetSection("DataSource").Value;
            }
        }
    }
}
