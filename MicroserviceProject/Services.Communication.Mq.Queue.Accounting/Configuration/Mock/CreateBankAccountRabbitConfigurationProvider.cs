
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Queue.Accounting.Configuration.Mock
{
    /// <summary>
    /// Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfını taklit eden sınıf
    /// </summary>
    public class CreateBankAccountRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static CreateBankAccountRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            return new CreateBankAccountRabbitConfiguration(configuration);
        }
    }
}
