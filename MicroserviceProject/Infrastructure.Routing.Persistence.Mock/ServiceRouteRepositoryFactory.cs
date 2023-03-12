using Infrastructure.Routing.Persistence.Abstract;
using Infrastructure.Routing.Persistence.Repositories.Sql;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Routing.Persistence.Mock
{
    /// <summary>
    /// Servis rota repository sınıfını taklit eden sınıf
    /// </summary>
    public class ServiceRouteRepositoryFactory
    {
        /// <summary>
        /// Servis rota repository nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static IServiceRouteRepository GetServiceRouteRepository(IConfiguration configuration)
        {
            return new SqlServiceRouteRepository(configuration);
        }
    }
}
