using Services.Communication.Mq.Queue.Buying.Configuration;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.Publishers.Mock
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
