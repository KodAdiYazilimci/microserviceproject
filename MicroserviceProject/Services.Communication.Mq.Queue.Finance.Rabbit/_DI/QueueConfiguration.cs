using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Finance.DI;
using Services.Communication.Mq.Queue.Finance.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Finance.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Finance.Rabbit.DI
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
        public static IServiceCollection RegisterFinanceQueuePublishers(this IServiceCollection services)
        {
            services.AddSingleton<InventoryRequestPublisher>();
            services.AddSingleton<ProductionRequestPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterFinanceQueueConsumers(this IServiceCollection services)
        {
            services.RegisterHttpFinanceDepartmentCommunicators();

            services.AddSingleton<InventoryRequestConsumer>();
            services.AddSingleton<ProductionRequestConsumer>();

            return services;
        }
    }
}
