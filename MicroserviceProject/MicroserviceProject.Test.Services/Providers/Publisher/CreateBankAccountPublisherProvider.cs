using MicroserviceProject.Services.Communication.Configuration.Rabbit.Accounting;
using MicroserviceProject.Services.Communication.Publishers.Account;

namespace MicroserviceProject.Test.Services.Providers.Publisher
{
    public class CreateBankAccountPublisherProvider
    {
        private static CreateBankAccountPublisher publisher = null;

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
