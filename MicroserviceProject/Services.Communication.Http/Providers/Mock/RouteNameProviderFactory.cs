
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Http.Providers.Mock
{
    /// <summary>
    /// Servis rota sağlayıcısını taklit eden sınıf
    /// </summary>
    public class RouteNameProviderFactory
    {
        /// <summary>
        /// Servis rota sağlayıcısı
        /// </summary>
        private static RouteNameProvider routeNameProvider = null;

        /// <summary>
        /// Servis rota sağlayıcısı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static RouteNameProvider GetRouteNameProvider(IConfiguration configuration)
        {
            if (routeNameProvider == null)
            {
                routeNameProvider = new RouteNameProvider(configuration);
            }

            return routeNameProvider;
        }
    }
}
