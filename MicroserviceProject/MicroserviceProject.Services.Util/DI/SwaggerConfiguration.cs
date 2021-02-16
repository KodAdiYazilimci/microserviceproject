using Microsoft.Extensions.DependencyInjection;

namespace MicroserviceProject.Services.Util.DI
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
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("CoreSwagger", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "MicroserviceProject.Services.Business.Departments.HR Swagger",
                    Version = "1.0.0",
                    Description = "ApiGateway+UI",
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
