
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Production.Services;

namespace Services.Api.Business.Departments.Production.DI
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
            services.AddScoped<ProductService>();
            services.AddScoped<ProductionService>();

            return services;
        }
    }
}
