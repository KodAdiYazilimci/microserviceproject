using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Selling.DI;
using Services.Communication.Mq.Queue.Selling.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Selling.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Selling.Rabbit.DI
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
        public static IServiceCollection RegisterSellingQueuePublishers(this IServiceCollection services)
        {
            services.AddSingleton<NotifyProductionRequestApprovementPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSellingQueueConsumers(this IServiceCollection services)
        {
            services.RegisterHttpSellingDepartmentCommunicators();

            services.AddSingleton<NotifyProductionRequestApprovementConsumer>();

            return services;
        }
    }
}
