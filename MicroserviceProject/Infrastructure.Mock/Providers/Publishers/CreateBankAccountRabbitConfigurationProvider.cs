using Infrastructure.Communication.Mq.Rabbit.Configuration.Accounting;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Publishers
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
