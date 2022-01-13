using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.IT.Configuration;
using Services.Communication.Mq.Rabbit.Queue.IT.Consumers;
using Services.Communication.Mq.Rabbit.Queue.IT.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.IT.DI
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
        public static IServiceCollection RegisterITQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<AssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<InformInventoryRequestRabbitConfiguration>();

            return services;
        }

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterITQueuePublishers(this IServiceCollection services)
        {
            RegisterITQueueConfigurations(services);

            services.AddSingleton<AssignInventoryToWorkerPublisher>();
            services.AddSingleton<InformInventoryRequestPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterITQueueConsumers(this IServiceCollection services)
        {
            RegisterITQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();

            services.AddSingleton<InformInventoryRequestConsumer>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();

            return services;
        }
    }
}
