using Communication.Mq.Rabbit.Publisher.Department.AA;
using Communication.Mq.Rabbit.Publisher.Department.Accounting;
using Communication.Mq.Rabbit.Publisher.Department.Buying;
using Communication.Mq.Rabbit.Publisher.Department.Finance;
using Communication.Mq.Rabbit.Publisher.Department.IT;
using Communication.Mq.Rabbit.Publisher.Department.Storage;

using Microsoft.Extensions.DependencyInjection;

namespace Communication.Mq.Rabbit.Publisher.Department.DI
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
        public static IServiceCollection RegisterPublishers(this IServiceCollection services)
        {
            services.AddSingleton<AAAssignInventoryToWorkerPublisher>();
            services.AddSingleton<ITAssignInventoryToWorkerPublisher>();
            services.AddSingleton<CreateBankAccountPublisher>();
            services.AddSingleton<CreateInventoryRequestPublisher>();
            services.AddSingleton<AAInformInventoryRequestPublisher>();
            services.AddSingleton<ITInformInventoryRequestPublisher>();
            services.AddSingleton<NotifyCostApprovementPublisher>();
            services.AddSingleton<InventoryRequestPublisher>();
            services.AddSingleton<ProductionRequestPublisher>();
            services.AddSingleton<DescendProductStockPublisher>();

            return services;
        }
    }
}
