
using Microsoft.Extensions.Configuration;

namespace Services.Communication.Mq.Queue.Buying.Configuration.Mock
{
    public class CreateInventoryRequestRabbitConfigurationProvider
    {
        public static CreateInventoryRequestRabbitConfiguration GetCreateInventoryRequestPublisher(IConfiguration configuration)
        {
            return new CreateInventoryRequestRabbitConfiguration(configuration);
        }
    }
}
