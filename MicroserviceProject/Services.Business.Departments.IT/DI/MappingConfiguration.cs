using AutoMapper;

using MicroserviceProject.Services.Business.Departments.IT.Configuration.Mapping;

using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Business.Departments.IT.DI
{
    /// <summary>
    /// Mapping için DI sınıfı
    /// </summary>
    public static class MappingConfiguration
    {
        /// <summary>
        /// Mappingleri enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterMappings(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());

            return services;
        }
    }
}
