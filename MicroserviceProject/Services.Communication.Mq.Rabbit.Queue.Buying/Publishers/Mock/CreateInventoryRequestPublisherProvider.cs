using Services.Communication.Mq.Rabbit.Queue.Buying.Configuration;

namespace Services.Communication.Mq.Rabbit.Queue.Buying.Publishers.Mock
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
