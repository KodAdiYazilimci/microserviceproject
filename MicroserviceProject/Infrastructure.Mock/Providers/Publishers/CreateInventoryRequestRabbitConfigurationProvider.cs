using Communication.Mq.Rabbit.Configuration.Department.Buying;

using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mock.Providers.Publishers
{
    public class CreateInventoryRequestRabbitConfigurationProvider
    {
        private static CreateInventoryRequestRabbitConfiguration publisher;

        public static CreateInventoryRequestRabbitConfiguration GetCreateInventoryRequestPublisher(IConfiguration configuration)
        {
            if (publisher == null)
            {
                publisher = new CreateInventoryRequestRabbitConfiguration(configuration);
            }

            return publisher;
        }
    }
}
