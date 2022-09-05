using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Util.DI
{
    /// <summary>
    /// Swagger DI sınıfı
    /// </summary>
    public static class SwaggerConfiguration
    {
        /// <summary>
        /// Swagger ı enjekte eder
        /// </summary>
        /// <param name="services">DI servisleri nesnesi</param>
        /// <returns></returns>
        public static IServiceCollection RegisterSwagger(this IServiceCollection services, string applicationName, string description)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CoreSwagger", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"{applicationName} Swagger",
                    Version = "1.0.0",
                    Description = description,
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "Swagger Implementation Serkan Camur",
                        Url = new System.Uri("http://serkancamur.com.tr"),
                        Email = "serkan@serkancamur.com.tr"
                    },
                    TermsOfService = new System.Uri("http://swagger.io/terms/")
                });
            });

            return services;
        }
    }
}
