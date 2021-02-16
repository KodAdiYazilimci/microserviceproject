
using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.UnitOfWork.DI
{
    /// <summary>
    /// İş birimi DI sınıfı
    /// </summary>
    public static class UnitOfWorkConfiguration
    {
        /// <summary>
        /// İş birimini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
