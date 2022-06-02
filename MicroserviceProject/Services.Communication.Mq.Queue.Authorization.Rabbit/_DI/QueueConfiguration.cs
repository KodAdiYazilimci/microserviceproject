using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Authorization.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Authorization.Rabbit.DI
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
        public static IServiceCollection RegisterAuthorizationQueuePublishers(this IServiceCollection services)
        {
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
            services.RegisterHttpAuthorizationCommunicators();

            services.AddSingleton<InformInvalidTokenConsumer>();

            return services;
        }
    }
}
