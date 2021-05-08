using MicroserviceProject.Services.Communication.Configuration.Rabbit.IT;
using MicroserviceProject.Services.Communication.Publishers.IT;

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
