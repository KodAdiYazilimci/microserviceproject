
using System.Configuration;

namespace Presentation.UI.Infrastructure.Communication.Broker.Providers
{
    /// <summary>
    /// Servis iletişimindeki yetki denetimi için kullanıcı bilgilerini sağlayan sınıf
    /// </summary>
    public class CredentialProvider
    {
        /// <summary>
        /// Servise ait e-posta adresi
        /// </summary>
        public string GetEmail
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Configuration.Authorization.Credential.email"]
                    .ToString();
            }
        }

        /// <summary>
        /// Servise ait parola
        /// </summary>
        public string GetPassword
        {
            get
            {
                return
                    ConfigurationManager
                    .AppSettings["Configuration.Authorization.Credential.password"]
                    .ToString();
            }
        }
    }
}
