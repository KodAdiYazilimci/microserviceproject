using MicroserviceProject.Services.Communication.Configuration.Rabbit.AA;
using MicroserviceProject.Services.Communication.Publishers.AA;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
