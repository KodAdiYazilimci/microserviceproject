using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Queue.Production.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Production.Consumers;
using Services.Communication.Mq.Rabbit.Queue.Production.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.Production.DI
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
        public static IServiceCollection RegisterProductionQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<ProductionProduceRabbitConfiguration>();

            return services;
        }

        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterProductionQueuePublishers(this IServiceCollection services)
        {
            RegisterProductionQueueConfigurations(services);

            services.AddSingleton<ProductionProducePublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterProductionQueueConsumers(this IServiceCollection services)
        {
            RegisterProductionQueueConfigurations(services);

            services.RegisterHttpDepartmentCommunicators();
            
            services.AddSingleton<ProduceConsumer>();

            return services;
        }
    }
}
