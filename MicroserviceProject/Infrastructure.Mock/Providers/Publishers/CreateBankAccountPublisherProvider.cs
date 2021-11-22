using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Services.Communication.Mq.Rabbit.Publisher.Department.Accounting;

namespace Infrastructure.Mock.Publishers
{
    /// <summary>
    /// Çalışana banka hesabı oluşturan yayıncıyı taklit eden sınıf
    /// </summary>
    public class CreateBankAccountPublisherProvider
    {
        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncı
        /// </summary>
        private static CreateBankAccountPublisher publisher = null;

        /// <summary>
        /// Çalışana banka hesabı oluşturan yayıncıyı verir
        /// </summary>
        /// <param name="rabbitConfiguration">Çalışana banka hesabı oluşturan yayıncının yapılandırma sınıfı nesnesi</param>
        /// <returns></returns>
        public static CreateBankAccountPublisher GetPublisher(CreateBankAccountRabbitConfiguration rabbitConfiguration)
        {
            if (publisher == null)
            {
                publisher = new CreateBankAccountPublisher(rabbitConfiguration);
            }

            return publisher;
        }
    }
}
