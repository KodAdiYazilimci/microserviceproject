using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.AA;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.AA;

namespace MicroserviceProject.Test.Services.Providers.Publisher
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
