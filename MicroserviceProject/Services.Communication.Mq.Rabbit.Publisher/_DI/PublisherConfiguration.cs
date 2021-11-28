using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Rabbit.Configuration.DI;
using Services.Communication.Mq.Rabbit.Publisher.Authorization;
using Services.Communication.Mq.Rabbit.Publisher.Department.Accounting;
using Services.Communication.Mq.Rabbit.Publisher.Department.Buying;
using Services.Communication.Mq.Rabbit.Publisher.Department.Finance;
using Services.Communication.Mq.Rabbit.Publisher.Department.Production;
using Services.Communication.Mq.Rabbit.Publisher.Department.Selling;
using Services.Communication.Mq.Rabbit.Publisher.Department.Storage;

namespace Services.Communication.Mq.Rabbit.Publisher.Department.DI
{
    /// <summary>
    /// Kuyrukların DI sınıfı
    /// </summary>
    public static class PublisherConfiguration
    {
        /// <summary>
        /// Kuyrukları enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterQueuePublishers(this IServiceCollection services)
        {
            services.RegisterQueueConfigurations();

            services.AddSingleton<InformInvalidTokenPublisher>();

            services.AddSingleton<AA.AssignInventoryToWorkerPublisher>();
            services.AddSingleton<AA.InformInventoryRequestPublisher>();
            services.AddSingleton<CreateBankAccountPublisher>();
            services.AddSingleton<CreateInventoryRequestPublisher>();
            services.AddSingleton<CreateProductRequestPublisher>();
            services.AddSingleton<DescendProductStockPublisher>();
            services.AddSingleton<IncreaseProductStockPublisher>();
            services.AddSingleton<InventoryRequestPublisher>();
            services.AddSingleton<IT.AssignInventoryToWorkerPublisher>();
            services.AddSingleton<IT.InformInventoryRequestPublisher>();
            services.AddSingleton<NotifyCostApprovementPublisher>();
            services.AddSingleton<NotifyProductionRequestApprovementPublisher>();
            services.AddSingleton<ProductionProducePublisher>();
            services.AddSingleton<ProductionRequestPublisher>();

            return services;
        }
    }
}
