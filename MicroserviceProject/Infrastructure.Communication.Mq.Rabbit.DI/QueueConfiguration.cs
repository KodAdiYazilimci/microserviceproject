using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.AA;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Accounting;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Buying;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Finance;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.IT;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.DI
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
            services.AddSingleton<AAAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<ITAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<CreateBankAccountRabbitConfiguration>();
            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<AAInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<ITInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();
            services.AddSingleton<InventoryRequestRabbitConfiguration>();

            return services;
        }
    }
}
