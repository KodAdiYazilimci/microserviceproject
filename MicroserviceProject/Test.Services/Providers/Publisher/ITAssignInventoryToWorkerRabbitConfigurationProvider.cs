using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.IT;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Test.Services.Providers.Publisher
{
    public class ITAssignInventoryToWorkerRabbitConfigurationProvider
    {
        private static ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration = null;

        public static ITAssignInventoryToWorkerRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new ITAssignInventoryToWorkerRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
