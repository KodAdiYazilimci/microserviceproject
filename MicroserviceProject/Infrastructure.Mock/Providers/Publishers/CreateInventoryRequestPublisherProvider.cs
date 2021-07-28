using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Communication.Mq.Rabbit.Publisher.Department.Buying;

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
