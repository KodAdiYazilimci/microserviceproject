using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Rabbit.Configuration.Authorization;
using Services.Communication.Mq.Rabbit.Configuration.Department.AA;
using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Services.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Services.Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.Communication.Mq.Rabbit.Configuration.Department.Production;
using Services.Communication.Mq.Rabbit.Configuration.Department.Selling;
using Services.Communication.Mq.Rabbit.Configuration.Department.Storage;

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
            services.AddSingleton<InformInvalidTokenRabbitConfiguration>();

            services.AddSingleton<Department.AA.AssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<Department.AA.InformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateBankAccountRabbitConfiguration>();
            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateProductRequestRabbitConfiguration>();
            services.AddSingleton<DescendProductStockRabbitConfiguration>();
            services.AddSingleton<IncreaseProductStockRabbitConfiguration>();
            services.AddSingleton<InventoryRequestRabbitConfiguration>();
            services.AddSingleton<Department.IT.AssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<Department.IT.InformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();
            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();
            services.AddSingleton<ProductionProduceRabbitConfiguration>();
            services.AddSingleton<ProductionRequestRabbitConfiguration>();

            return services;
        }
    }
}
