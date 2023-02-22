using Services.Communication.Mq.Queue.Accounting.Configuration;
using Services.Communication.Mq.Queue.Accounting.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Accounting.Rabbit.Publisher.Mock
{
    /// <summary>
    /// Çalışana banka hesabı oluşturan yayıncıyı taklit eden sınıf
    /// </summary>
    public class CreateBankAccountPublisherProvider
    {
        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncıyı verir
        /// </summary>
        /// <param name="rabbitConfiguration">Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfı nesnesi</param>
        /// <returns></returns>
        public static CreateBankAccountPublisher GetPublisher(CreateBankAccountRabbitConfiguration rabbitConfiguration)
        {
            return new CreateBankAccountPublisher(rabbitConfiguration);
        }
    }
}
