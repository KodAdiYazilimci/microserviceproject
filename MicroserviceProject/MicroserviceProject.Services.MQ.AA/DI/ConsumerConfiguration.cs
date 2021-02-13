
using MicroserviceProject.Services.Configuration.Communication.Rabbit.AA;
using MicroserviceProject.Services.MQ.AA.Util.Consumers.Inventory;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MicroserviceProject.Services.MQ.AA.DI
{
    /// <summary>
    /// Rabbit kuyruk tüketici sınıfların DI sınıfı
    /// </summary>
    public static class ConsumerConfiguration
    {
        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="hostBuilder">Hosting nesnesi</param>
        /// <returns></returns>
        public static IHost RegisterConsumers(this IHost hostBuilder)
        {

            IConfiguration configuration =
                (IConfiguration)hostBuilder.Services.GetService(typeof(IConfiguration));

            AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration =
                (AssignInventoryToWorkerRabbitConfiguration)hostBuilder.Services.GetService(typeof(AssignInventoryToWorkerRabbitConfiguration));

            AssignInventoryToWorkerConsumer assignInventoryToWorkerConsumer =
                new AssignInventoryToWorkerConsumer(rabbitConfiguration);

            assignInventoryToWorkerConsumer.StartToConsume();


            return hostBuilder;
        }
    }
}
