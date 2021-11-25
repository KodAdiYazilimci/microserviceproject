
using Infrastructure.Security.Authentication.Providers;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Security.Authentication.Mock
{
    /// <summary>
    /// Kullanıcı bilgi sağlayıcısını taklit eder
    /// </summary>
    public class CredentialProviderFactory
    {
        /// <summary>
        /// Kullanıcı bilgi sağlayıcısı
        /// </summary>
        private static CredentialProvider credentialProvider;

        /// <summary>
        /// Kullanıcı bilgi sağlayıcısını verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static CredentialProvider GetCredentialProvider(IConfiguration configuration)
        {
            if (credentialProvider == null)
            {
                credentialProvider = new CredentialProvider(configuration);
            }

            return credentialProvider;
        }
    }
}
