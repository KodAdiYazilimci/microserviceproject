using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Accounting;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.Accounting;

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
