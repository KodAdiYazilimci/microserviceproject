using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.IT;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.IT;

namespace MicroserviceProject.Test.Services.Providers.Publisher
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
