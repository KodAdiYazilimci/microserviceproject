using Microsoft.Extensions.DependencyInjection;

using Services.Communication.Http.Broker.Department.CR.Abstract;
using Services.Communication.Http.Broker.Department.DI;

namespace Services.Communication.Http.Broker.Department.CR.DI
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
        public static IServiceCollection RegisterHttpCRDepartmentCommunicators(this IServiceCollection services)
        {
            services.RegisterDepartmentCommunicator();
            services.AddSingleton<ICRCommunicator, CRCommunicator>();

            return services;
        }
    }
}
