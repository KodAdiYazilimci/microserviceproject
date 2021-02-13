using MicroserviceProject.Services.Business.Util.Communication.Rabbit.AA;
using MicroserviceProject.Services.Configuration.Communication.Rabbit.AA;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.DI
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
        public static IServiceCollection RegisterQueues(this IServiceCollection services)
        {
            services.AddSingleton<AssignInventoryToWorkerRabbitConfiguration>();

            services.AddSingleton<AssignInventoryToWorkerPublisher>();

            return services;
        }
    }
}
