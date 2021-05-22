using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.AA;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.IT;

using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Communication.Mq.Rabbit.Configuration.DI
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
