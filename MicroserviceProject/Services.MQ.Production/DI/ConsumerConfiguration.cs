using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Production;

using Microsoft.Extensions.DependencyInjection;

using Services.MQ.Production.Util.Consumers.Request;

namespace Services.MQ.Production.DI
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
            services.AddSingleton<ProductionProduceRabbitConfiguration>();

            services.AddSingleton<ProduceConsumer>();

            return services;
        }
    }
}
