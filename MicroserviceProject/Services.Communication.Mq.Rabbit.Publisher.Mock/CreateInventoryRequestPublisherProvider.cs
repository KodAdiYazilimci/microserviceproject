using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Services.Communication.Mq.Rabbit.Publisher.Department.Buying;

namespace Services.Communication.Mq.Rabbit.Publisher.Mock
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
