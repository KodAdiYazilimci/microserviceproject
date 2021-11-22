using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;

using Microsoft.Extensions.DependencyInjection;

using Services.MQ.Buying.Util.Consumers.Cost;
using Services.MQ.Buying.Util.Consumers.Request;

namespace Services.MQ.Buying.DI
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
            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();

            services.AddSingleton<CreateInventoryRequestConsumer>();
            services.AddSingleton<NotifyCostApprovementConsumer>();

            return services;
        }
    }
}
