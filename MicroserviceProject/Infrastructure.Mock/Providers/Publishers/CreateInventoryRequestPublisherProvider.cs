using Infrastructure.Communication.Mq.Rabbit.Configuration.Buying;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Buying;

namespace Infrastructure.Mock.Providers.Publishers
{
    public class CreateInventoryRequestPublisherProvider
    {
        private static CreateInventoryRequestPublisher publisher;

        public static CreateInventoryRequestPublisher GetCreateInventoryRequestPublisher(CreateInventoryRequestRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new CreateInventoryRequestPublisher(configuration);
            }

            return publisher;
        }
    }
}
