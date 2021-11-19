using Communication.Mq.Rabbit.Configuration.Department.Selling;

using Microsoft.Extensions.DependencyInjection;

using Services.MQ.Buying.Util.Consumers.Cost;

namespace Services.MQ.Selling.DI
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
            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();

            services.AddSingleton<NotifyProductionRequestApprovementConsumer>();

            return services;
        }
    }
}
