using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Authorization.DI
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
        public static IServiceCollection RegisterAuthorizationQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<InformInvalidTokenRabbitConfiguration>();

            return services;
        }

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAuthorizationQueuePublishers(this IServiceCollection services)
        {
            RegisterAuthorizationQueueConfigurations(services);

            services.AddSingleton<InformInvalidTokenPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAuthorizationQueueConsumers(this IServiceCollection services)
        {
            RegisterAuthorizationQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();

            services.AddSingleton<InformInvalidTokenConsumer>();

            return services;
        }
    }
}
