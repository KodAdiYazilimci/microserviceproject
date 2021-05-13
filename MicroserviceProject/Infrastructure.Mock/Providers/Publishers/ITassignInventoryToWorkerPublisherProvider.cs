using Infrastructure.Communication.Mq.Rabbit.Configuration.IT;
using Infrastructure.Communication.Mq.Rabbit.Publisher.IT;

namespace Infrastructure.Mock.Publishers
{
    public class ITassignInventoryToWorkerPublisherProvider
    {
        private static ITAssignInventoryToWorkerPublisher publisher = null;

        public static ITAssignInventoryToWorkerPublisher GetPublisher(ITAssignInventoryToWorkerRabbitConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new ITAssignInventoryToWorkerPublisher(configuration);
            }

            return publisher;
        }
    }
}
