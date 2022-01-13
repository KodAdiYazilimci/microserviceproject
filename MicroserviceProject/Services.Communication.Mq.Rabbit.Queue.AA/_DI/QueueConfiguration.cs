using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.AA.DI;
using Services.Communication.Mq.Rabbit.Queue.AA.Configuration;
using Services.Communication.Mq.Rabbit.Queue.AA.Consumers;
using Services.Communication.Mq.Rabbit.Queue.AA.Publishers;

namespace Services.Communication.Mq.Rabbit.Queue.AA.DI
{
    /// <summary>
    /// Kuyrukların DI sınıfı
    /// </summary>
    public static class AAQueueConfiguration
    {
        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAAQueueConfigurations(this IServiceCollection services)
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
        public static IServiceCollection RegisterAAQueuePublishers(this IServiceCollection services)
        {
            RegisterAAQueueConfigurations(services);

            services.AddSingleton<AssignInventoryToWorkerPublisher>();
            services.AddSingleton<InformInventoryRequestPublisher>();

            return services;
        }

        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterAAQueueConsumers(this IServiceCollection services)
        {
            RegisterAAQueueConfigurations(services);

            services.RegisterHttpAADepartmentCommunicators();            

            services.AddSingleton<AssignInventoryToWorkerConsumer>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            return services;
        }
    }
}
