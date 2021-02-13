
using MicroserviceProject.Services.Configuration.Communication.Rabbit.AA;
using MicroserviceProject.Services.MQ.AA.Util.Consumers.Inventory;

using Microsoft.Extensions.DependencyInjection;

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
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterConsumers(this IServiceCollection services)
        {
            services.AddSingleton<AssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();


            return services;
        }
    }
}
