using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Finance.Configuration;

namespace Services.Communication.Mq.Queue.Finance.DI
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
        public static IServiceCollection RegisterFinanceQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<InventoryRequestRabbitConfiguration>();
            services.AddSingleton<ProductionRequestRabbitConfiguration>();

            return services;
        }
    }
}
