using Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.MQ.IT.Util.Consumers.Inventory;

using Microsoft.Extensions.DependencyInjection;

namespace Services.MQ.IT.DI
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
            services.AddSingleton<ITInformInventoryRequestRabbitConfiguration>();

            services.AddSingleton<AssignInventoryToWorkerConsumer>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            return services;
        }
    }
}
