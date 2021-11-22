using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Http.Broker.Department.Accounting;
using Services.Communication.Http.Broker.Department.Buying;
using Services.Communication.Http.Broker.Department.CR;
using Services.Communication.Http.Broker.Department.Finance;
using Services.Communication.Http.Broker.Department.HR;
using Services.Communication.Http.Broker.Department.IT;
using Services.Communication.Http.Broker.Department.Production;
using Services.Communication.Http.Broker.Department.Selling;
using Services.Communication.Http.Broker.Department.Storage;

using Microsoft.Extensions.DependencyInjection;

namespace Services.Communication.Http.Broker.Department.DI
{
    /// <summary>
    /// İletişimcilerin DI sınıfı
    /// </summary>
    public static class CommunicatorConfiguration
    {
        /// <summary>
        /// İletişimcileri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterDepartmentCommunicators(this IServiceCollection services)
        {
            services.AddSingleton<AACommunicator>();
            services.AddSingleton<AccountingCommunicator>();
            services.AddSingleton<BuyingCommunicator>();
            services.AddSingleton<CRCommunicator>();
            services.AddSingleton<FinanceCommunicator>();
            services.AddSingleton<HRCommunicator>();
            services.AddSingleton<ITCommunicator>();
            services.AddSingleton<ProductionCommunicator>();
            services.AddSingleton<SellingCommunicator>();
            services.AddSingleton<StorageCommunicator>();
                        
            return services;
        }
    }
}
