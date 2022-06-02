using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Buying.DI;
using Services.Communication.Mq.Queue.Buying.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Buying.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.DI
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
        public static IServiceCollection RegisterBuyingQueuePublishers(this IServiceCollection services)
        {
            services.AddSingleton<CreateInventoryRequestPublisher>();
            services.AddSingleton<CreateProductRequestPublisher>();          
            services.AddSingleton<NotifyCostApprovementPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterBuyingQueueConsumers(this IServiceCollection services)
        {
            services.RegisterHttpBuyingDepartmentCommunicators();

            services.AddSingleton<CreateInventoryRequestConsumer>();
            services.AddSingleton<NotifyCostApprovementConsumer>();

            return services;
        }
    }
}
