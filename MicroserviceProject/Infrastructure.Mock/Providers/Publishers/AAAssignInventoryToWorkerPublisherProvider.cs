using Infrastructure.Communication.Mq.Rabbit.Configuration.AA;
using Infrastructure.Communication.Mq.Rabbit.Publisher.AA;

namespace Infrastructure.Mock.Publishers
{
    public class AAAssignInventoryToWorkerPublisherProvider
    {
        private static AAAssignInventoryToWorkerPublisher publisher = null;

        public static AAAssignInventoryToWorkerPublisher GetPublisher(AAAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new AAAssignInventoryToWorkerPublisher(configuration);
            }

            return publisher;
        }
    }
}
