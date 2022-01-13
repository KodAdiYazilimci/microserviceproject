using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.Finance.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Finance.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Finance.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Finance.DI
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

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterFinanceQueuePublishers(this IServiceCollection services)
        {
            RegisterFinanceQueueConfigurations(services);

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
            RegisterFinanceQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();

            services.AddSingleton<InventoryRequestConsumer>();
            services.AddSingleton<ProductionRequestConsumer>();

            return services;
        }
    }
}
