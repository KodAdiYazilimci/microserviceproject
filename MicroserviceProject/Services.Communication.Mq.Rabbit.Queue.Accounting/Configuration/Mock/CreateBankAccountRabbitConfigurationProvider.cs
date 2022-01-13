
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.Accounting.Configuration.Mock
{
    /// <summary>
    /// Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfını taklit eden sınıf
    /// </summary>
    public class CreateBankAccountRabbitConfigurationProvider
    {
        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfı nesnesi
        /// </summary>
        private static CreateBankAccountRabbitConfiguration rabbitConfiguration;

        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfı nesnesini verir
        /// </summary>
        /// <param name="configuration">Yapılandırma arayüzü nesnesi</param>
        /// <returns></returns>
        public static CreateBankAccountRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new CreateBankAccountRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
