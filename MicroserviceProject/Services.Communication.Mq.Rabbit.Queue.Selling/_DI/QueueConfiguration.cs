using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Selling.DI;
using Services.Communication.Mq.Rabbit.Queue.Selling.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Selling.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Selling.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Selling.DI
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
        public static IServiceCollection RegisterSellingQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();

            return services;
        }

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSellingQueuePublishers(this IServiceCollection services)
        {
            RegisterSellingQueueConfigurations(services);

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
            RegisterSellingQueueConfigurations(services);

            services.RegisterHttpSellingDepartmentCommunicators();

            services.AddSingleton<NotifyProductionRequestApprovementConsumer>();

            return services;
        }
    }
}
