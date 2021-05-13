using Infrastructure.Communication.Mq.Rabbit.Configuration.AA;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Publishers
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
