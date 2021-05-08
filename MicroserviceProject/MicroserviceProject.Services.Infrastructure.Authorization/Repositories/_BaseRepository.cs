using Microsoft.Extensions.Configuration;

using System;

namespace MicroserviceProject.Services.Infrastructure.Authorization.Persistence.Sql.Repositories
{
    /// <summary>
    /// Repository sınıfların temel sınıfı
    /// </summary>
    public abstract class BaseRepository : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
                    .GetSection("Authorization")["DataSource"];
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
        public virtual void Dispose(bool disposing)
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
