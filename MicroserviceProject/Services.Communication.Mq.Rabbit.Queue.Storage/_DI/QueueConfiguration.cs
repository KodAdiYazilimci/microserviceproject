using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.Storage.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Storage.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Storage.Publishers;

namespace Services.Communication.Mq.Rabbit.Configuration.DI
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

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterStorageQueuePublishers(this IServiceCollection services)
        {
            RegisterStorageQueueConfigurations(services);

            services.AddSingleton<DescendProductStockPublisher>();
            services.AddSingleton<IncreaseProductStockPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterStorageQueueConsumers(this IServiceCollection services)
        {
            RegisterStorageQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();

            services.AddSingleton<DescendProductStockConsumer>();

            return services;
        }
    }
}
