using Infrastructure.Sockets.Persistence.Repositories.Sql;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Sockets.Persistence.Mock
{
    /// <summary>
    /// Web soketler repository sınıfını taklit eder
    /// </summary>
    public class SocketRepositoryFactory
    {
        /// <summary>
        /// Web soketler repository nesnesi
        /// </summary>
        private static SocketRepository socketRepository;

        /// <summary>
        /// Web soketler repository sınıfı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static SocketRepository GetSocketRepository(IConfiguration configuration)
        {
            if (socketRepository == null)
            {
                socketRepository = new SocketRepository(configuration);
            }

            return socketRepository;
        }
    }
}
