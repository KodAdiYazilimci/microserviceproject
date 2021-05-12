using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.AA;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Test.Services.Providers.Publisher
{
    public class AAAssignInventoryToWorkerRabbitConfigurationProvider
    {
        private static AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        public static AAAssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new AAAssignInventoryToWorkerRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
