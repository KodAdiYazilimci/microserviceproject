using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Storage.DI;
using Services.Communication.Mq.Queue.Storage.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Storage.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Storage.Rabbit.Configuration.DI
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
        public static IServiceCollection RegisterStorageQueuePublishers(this IServiceCollection services)
        {
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
            services.RegisterHttpStorageDepartmentCommunicators();

            services.AddSingleton<DescendProductStockConsumer>();

            return services;
        }
    }
}
