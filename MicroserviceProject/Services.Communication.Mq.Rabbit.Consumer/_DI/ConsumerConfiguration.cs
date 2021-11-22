using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Mq.Rabbit.Configuration.Department.AA;
using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Services.Communication.Mq.Rabbit.Configuration.Department.Buying;
using Services.Communication.Mq.Rabbit.Configuration.Department.Finance;
using Services.Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.Communication.Mq.Rabbit.Configuration.Department.Production;
using Services.Communication.Mq.Rabbit.Configuration.Department.Selling;
using Services.Communication.Mq.Rabbit.Configuration.Department.Storage;
using Services.Communication.Mq.Rabbit.Consumer.Department.AA;
using Services.Communication.Mq.Rabbit.Consumer.Department.Accounting;
using Services.Communication.Mq.Rabbit.Consumer.Department.Buying;
using Services.Communication.Mq.Rabbit.Consumer.Department.Finance;
using Services.Communication.Mq.Rabbit.Consumer.Department.Production;
using Services.Communication.Mq.Rabbit.Consumer.Department.Selling;
using Services.Communication.Mq.Rabbit.Consumer.Department.Storage;

namespace Services.Communication.Mq.Rabbit.Consumer.DI
{
    /// <summary>
    /// Rabbit kuyruk tüketici sınıfların DI sınıfı
    /// </summary>
    public static class ConsumerConfiguration
    {
        /// <summary>
        /// Rabbit kuyruk tüketicilerini enjekte eder
        /// </summary>
        /// <param name="services">DI sınıfları nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterConsumers(this IServiceCollection services)
        {

            services.AddSingleton<AAAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();

            services.AddSingleton<AAInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            services.AddSingleton<CreateBankAccountRabbitConfiguration>();
            services.AddSingleton<CreateBankAccountConsumer>();

            services.AddSingleton<CreateInventoryRequestRabbitConfiguration>();
            services.AddSingleton<CreateInventoryRequestConsumer>();

            services.AddSingleton<NotifyCostApprovementRabbitConfiguration>();
            services.AddSingleton<NotifyCostApprovementConsumer>();

            services.AddSingleton<InventoryRequestRabbitConfiguration>();
            services.AddSingleton<InventoryRequestConsumer>();

            services.AddSingleton<ProductionRequestRabbitConfiguration>();
            services.AddSingleton<ProductionRequestConsumer>();

            services.AddSingleton<ITAssignInventoryToWorkerRabbitConfiguration>();
            services.AddSingleton<AssignInventoryToWorkerConsumer>();

            services.AddSingleton<ITInformInventoryRequestRabbitConfiguration>();
            services.AddSingleton<InformInventoryRequestConsumer>();

            services.AddSingleton<ProductionProduceRabbitConfiguration>();
            services.AddSingleton<ProduceConsumer>();

            services.AddSingleton<NotifyProductionRequestApprovementRabbitConfiguration>();
            services.AddSingleton<NotifyProductionRequestApprovementConsumer>();


            services.AddSingleton<DescendProductStockRabbitConfiguration>();
            services.AddSingleton<DescendProductStockConsumer>();

            return services;
        }
    }
}
