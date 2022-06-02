using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.Production.DI;
using Services.Communication.Mq.Queue.Production.Rabbit.Consumers;
using Services.Communication.Mq.Queue.Production.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.Production.Rabbit.DI
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
        public static IServiceCollection RegisterProductionQueuePublishers(this IServiceCollection services)
        {
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
            services.RegisterHttpProductionDepartmentCommunicators();
            
            services.AddSingleton<ProduceConsumer>();

            return services;
        }
    }
}
