using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Queue.AA.Configuration;

namespace Services.Communication.Mq.Queue.AA.DI
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
            services.AddSingleton<AAAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AAInformInventoryRequestRabbitConfiguration>();

            return services;
        }
    }
}
