using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Buying;
using MicroserviceProject.Services.MQ.Buying.Util.Consumers.Cost;
using MicroserviceProject.Services.MQ.Buying.Util.Consumers.Request;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.MQ.Buying.DI
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
