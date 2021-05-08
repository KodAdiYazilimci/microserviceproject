using MicroserviceProject.Services.Communication.Configuration.Rabbit.IT;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
