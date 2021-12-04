using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using Services.Api.Localization.Configuration.Mapping;

namespace Services.Api.Localization.DI
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
