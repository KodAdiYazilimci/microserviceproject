
using MicroserviceProject.Services.Communication.Configuration.Rabbit.IT;
using MicroserviceProject.Services.MQ.IT.Util.Consumers.Inventory;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.MQ.IT.DI
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
            services.AddSingleton<ITAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();


            return services;
        }
    }
}
