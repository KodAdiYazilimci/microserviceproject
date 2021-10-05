
using Microsoft.Extensions.DependencyInjection;

using Services.Business.Departments.Selling.Services;

namespace Services.Business.Departments.Selling.DI
{
    /// <summary>
    /// İş mantığı servisleri DI sınıfı
    /// </summary>
    public static class BusinessServicesConfiguration
    {
        /// <summary>
        /// İş mantığı servislerini enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<SellingService>();

            return services;
        }
    }
}
