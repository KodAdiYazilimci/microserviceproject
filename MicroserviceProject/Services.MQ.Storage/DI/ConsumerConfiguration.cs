using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Storage;

using Microsoft.Extensions.DependencyInjection;

using Services.MQ.Storage.Util.Consumers.ProductStock;

namespace Services.MQ.Storage.DI
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
            services.AddSingleton<DescendProductStockRabbitConfiguration>();

            services.AddSingleton<DescendProductStockConsumer>();

            return services;
        }
    }
}
