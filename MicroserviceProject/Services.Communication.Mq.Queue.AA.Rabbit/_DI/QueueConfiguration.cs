using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.AA.DI;
using Services.Communication.Mq.Queue.AA.Rabbit.Consumers;
using Services.Communication.Mq.Queue.AA.Rabbit.Publishers;

namespace Services.Communication.Mq.Queue.AA.Rabbit.DI
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
        public static IServiceCollection RegisterAAQueuePublishers(this IServiceCollection services)
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
        public static IServiceCollection RegisterAAQueueConsumers(this IServiceCollection services)
        {
            services.RegisterHttpAADepartmentCommunicators();            

            services.AddSingleton<AssignInventoryToWorkerConsumer>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            return services;
        }
    }
}
