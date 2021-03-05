using MicroserviceProject.Services.Communication.Configuration.Rabbit.AA;
using MicroserviceProject.Services.MQ.AA.Util.Consumers.Inventory;
using MicroserviceProject.Services.MQ.AA.Util.Consumers.Request;

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
            services.AddSingleton<AAAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AAInformInventoryRequestRabbitConfiguration>();

            services.AddSingleton<AssignInventoryToWorkerConsumer>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            return services;
        }
    }
}
