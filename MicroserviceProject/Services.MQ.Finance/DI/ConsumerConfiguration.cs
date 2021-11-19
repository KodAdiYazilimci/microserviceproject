using Communication.Mq.Rabbit.Configuration.Department.Finance;
using Services.MQ.Finance.Util.Consumers.Request;

using Microsoft.Extensions.DependencyInjection;

namespace Services.MQ.Finance.DI
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
            services.AddSingleton<ProductionRequestRabbitConfiguration>();

            services.AddSingleton<InventoryRequestConsumer>();
            services.AddSingleton<ProductionRequestConsumer>();

            return services;
        }
    }
}
