using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.Buying.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Buying.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Buying.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Buying.DI
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

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterBuyingQueuePublishers(this IServiceCollection services)
        {
            RegisterBuyingQueueConfigurations(services);

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
            RegisterBuyingQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();

            services.AddSingleton<CreateInventoryRequestConsumer>();
            services.AddSingleton<NotifyCostApprovementConsumer>();

            return services;
        }
    }
}
