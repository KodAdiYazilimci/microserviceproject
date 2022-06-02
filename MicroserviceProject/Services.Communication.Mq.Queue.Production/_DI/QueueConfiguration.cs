using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.Production.Configuration;

namespace Services.Communication.Mq.Queue.Production.DI
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
        public static IServiceCollection RegisterProductionQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<ProductionProduceRabbitConfiguration>();

            return services;
        }
    }
}
