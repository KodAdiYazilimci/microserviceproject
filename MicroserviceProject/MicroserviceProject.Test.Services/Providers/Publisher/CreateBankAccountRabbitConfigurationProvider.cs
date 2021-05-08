using MicroserviceProject.Services.Communication.Configuration.Rabbit.Accounting;

using Microsoft.Extensions.Configuration;

namespace MicroserviceProject.Test.Services.Providers.Publisher
{
    public class CreateBankAccountRabbitConfigurationProvider
    {
        private static CreateBankAccountRabbitConfiguration rabbitConfiguration;

        public static CreateBankAccountRabbitConfiguration GetConfiguration(IConfiguration configuration)
        {
            if (rabbitConfiguration == null)
            {
                rabbitConfiguration = new CreateBankAccountRabbitConfiguration(configuration);
            }

            return rabbitConfiguration;
        }
    }
}
