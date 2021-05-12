using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.AA;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.Accounting;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.Buying;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.Finance;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.IT;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.DI
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

            return services;
        }
    }
}
