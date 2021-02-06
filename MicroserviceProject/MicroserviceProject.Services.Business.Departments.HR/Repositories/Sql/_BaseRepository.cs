using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Services.Business.Departments.HR.Repositories.Sql
{
    /// <summary>
    /// Repository sınıfları için temel sınıf
    /// </summary>
    public class BaseRepository
    {
        /// <summary>
        /// Repository yapılandırmaları için configuration nesnesi
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Repository sınıfları için temel sınıf
        /// </summary>
        /// <param name="configuration">Repository yapılandırmaları için configuration nesnesi</param>
        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Repository sınıflarda kullanılacak veritabanı bağlantı cümlesi
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return _configuration
                    .GetSection("Persistence")
                    .GetSection("DataSource").Value;
            }
        }
    }
}
