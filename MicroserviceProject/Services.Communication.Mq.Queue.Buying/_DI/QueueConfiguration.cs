using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Buying.Configuration;

namespace Services.Communication.Mq.Queue.Buying.DI
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
        public static IServiceCollection RegisterBuyingQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateProductRequestRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();

            return services;
        }       
    }
}
