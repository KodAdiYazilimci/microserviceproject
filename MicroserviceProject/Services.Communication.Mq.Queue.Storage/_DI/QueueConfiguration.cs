using Microsoft.Extensions.DependencyInjection;

namespace Services.Communication.Mq.Queue.Storage.Configuration.DI
{
    /// <summary>
    /// Kuyrukların DI sınıfı
    /// </summary>
    public static class QueueConfiguration
    {
        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterStorageQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<DescendProductStockRabbitConfiguration>();
            services.AddSingleton<IncreaseProductStockRabbitConfiguration>();

            return services;
        }
    }
}
