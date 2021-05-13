using Infrastructure.Communication.Mq.Rabbit.Configuration.Accounting;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Accounting;

namespace Infrastructure.Mock.Publishers
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
