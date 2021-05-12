using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Finance;
using MicroserviceProject.Services.MQ.Finance.Util.Consumers.Request;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.MQ.Finance.DI
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
            services.AddSingleton<InventoryRequestRabbitConfiguration>();

            services.AddSingleton<InventoryRequestConsumer>();

            return services;
        }
    }
}
