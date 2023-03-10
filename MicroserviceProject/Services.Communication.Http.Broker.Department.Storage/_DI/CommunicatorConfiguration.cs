using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.DI;
using Services.Communication.Http.Broker.Department.Storage.Abstract;

namespace Services.Communication.Http.Broker.Department.Storage.DI
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
        public static IServiceCollection RegisterHttpStorageDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();
            services.AddSingleton<IStorageCommunicator, StorageCommunicator>();

            return services;
        }
    }
}
