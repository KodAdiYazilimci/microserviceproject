using Communication.Http.Authorization;
using Communication.Http.Department.AA;
using Communication.Http.Department.Accounting;
using Communication.Http.Department.Buying;
using Communication.Http.Department.CR;
using Communication.Http.Department.Finance;
using Communication.Http.Department.HR;
using Communication.Http.Department.IT;
using Communication.Http.Department.Production;
using Communication.Http.Department.Selling;
using Communication.Http.Department.Storage;

using Microsoft.Extensions.DependencyInjection;

namespace Communication.Http.DI
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
        public static IServiceCollection RegisterCommunicators(this IServiceCollection services)
        {
            services.AddSingleton<AuthorizationCommunicator>();
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
