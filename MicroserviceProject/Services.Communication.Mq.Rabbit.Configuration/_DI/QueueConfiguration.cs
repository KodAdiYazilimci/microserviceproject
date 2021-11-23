using Services.Communication.Mq.Rabbit.Configuration.Department.AA;
using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Services.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Services.Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.Communication.Mq.Rabbit.Configuration.Department.Production;
using Services.Communication.Mq.Rabbit.Configuration.Department.Selling;
using Services.Communication.Mq.Rabbit.Configuration.Department.Storage;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Communication.Mq.Rabbit.Configuration.DI
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
        public static IServiceCollection RegisterQueueConfigurations(this IServiceCollection services)
        {
            services.AddSingleton<AAAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AAInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateBankAccountRabbitConfiguration>();
            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateProductRequestRabbitConfiguration>();
            services.AddSingleton<DescendProductStockRabbitConfiguration>();
            services.AddSingleton<IncreaseProductStockRabbitConfiguration>();
            services.AddSingleton<InventoryRequestRabbitConfiguration>();
            services.AddSingleton<ITAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<ITInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();
            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();
            services.AddSingleton<ProductionProduceRabbitConfiguration>();
            services.AddSingleton<ProductionRequestRabbitConfiguration>();

            return services;
        }
    }
}
