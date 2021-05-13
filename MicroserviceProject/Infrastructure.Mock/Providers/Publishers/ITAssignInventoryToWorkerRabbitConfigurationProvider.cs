using Infrastructure.Communication.Mq.Rabbit.Configuration.IT;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Publishers
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
