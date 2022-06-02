using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Selling.Configuration;

namespace Services.Communication.Mq.Queue.Selling.DI
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
        public static IServiceCollection RegisterSellingQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();

            return services;
        }
    }
}
