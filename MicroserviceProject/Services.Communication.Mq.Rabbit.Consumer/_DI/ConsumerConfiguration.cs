using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Mq.Rabbit.Configuration.DI;
using Services.Communication.Mq.Rabbit.Consumer.Authorization;
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
        public static IServiceCollection RegisterQueueConsumers(this IServiceCollection services)
        {
            services.RegisterHttpDepartmentCommunicators();
            services.RegisterQueueConfigurations();

            services.AddSingleton<InformInvalidTokenConsumer>();

            services.AddSingleton<Department.AA.AssignInventoryToWorkerConsumer>();
            services.AddSingleton<Department.AA.InformInventoryRequestConsumer>();
            services.AddSingleton<CreateBankAccountConsumer>();
            services.AddSingleton<CreateInventoryRequestConsumer>();
            services.AddSingleton<DescendProductStockConsumer>();
            services.AddSingleton<InventoryRequestConsumer>();
            services.AddSingleton<NotifyCostApprovementConsumer>();
            services.AddSingleton<NotifyProductionRequestApprovementConsumer>();
            services.AddSingleton<ProductionRequestConsumer>();
            services.AddSingleton<Department.IT.InformInventoryRequestConsumer>();
            services.AddSingleton<Department.IT.AssignInventoryToWorkerConsumer>();
            services.AddSingleton<ProduceConsumer>();

            return services;
        }
    }
}
