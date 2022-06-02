using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.IT.DI;
using Services.Communication.Mq.Queue.IT.Rabbit.Consumers;
using Services.Communication.Mq.Queue.IT.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.IT.Rabbit.DI
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
        public static IServiceCollection RegisterITQueuePublishers(this IServiceCollection services)
        {
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
            services.RegisterHttpITDepartmentCommunicators();

            services.AddSingleton<InformInventoryRequestConsumer>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();

            return services;
        }
    }
}
