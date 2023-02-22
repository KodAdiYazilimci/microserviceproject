using Services.Communication.Mq.Queue.Buying.Configuration;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.Publishers.Mock
{
    public class CreateInventoryRequestPublisherProvider
    {
        public static CreateInventoryRequestPublisher GetCreateInventoryRequestPublisher(CreateInventoryRequestRabbitConfiguration configuration)
        {
            return new CreateInventoryRequestPublisher(configuration);
        }
    }
}
