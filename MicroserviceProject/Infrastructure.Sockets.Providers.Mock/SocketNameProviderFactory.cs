
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Sockets.Providers.Mock
{
    /// <summary>
    /// Web soket isim sağlayıcısını taklit eden sınıf
    /// </summary>
    public class SocketNameProviderFactory
    {
        /// <summary>
        /// Web soket isim sağlayıcısı
        /// </summary>
        private static SocketNameProvider socketNameProvider;

        /// <summary>
        /// Web soket isim sağlayıcısı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static SocketNameProvider GetSocketNameProvider(IConfiguration configuration)
        {
            if (socketNameProvider == null)
            {
                socketNameProvider = new SocketNameProvider(configuration);
            }

            return socketNameProvider;
        }
    }
}
