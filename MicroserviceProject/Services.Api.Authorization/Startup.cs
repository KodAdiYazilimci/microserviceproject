
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Authorization.DI;
using Services.Api.Infrastructure.Authorization.Configuration.Persistence;
using Services.Api.Infrastructure.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.Rabbit.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.UnitOfWork.EntityFramework.DI;
using Services.Util.Exception.Handlers;

namespace Services.Api.Infrastructure.Authorization
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterAuthorizationQueueConfigurations();
            services.RegisterAuthorizationQueuePublishers();
            services.RegisterBusinessServices();
            services.RegisterEntityFrameworkUnitOfWork<AuthContext>();
            services.RegisterExceptionLogger();
            services.RegisterInMemoryCaching();
            services.RegisterLocalizationProviders();
            services.RegisterPersistence();
            services.RegisterRepositories();
            services.RegisterRequestResponseLogger();
            services.RegisterSqlHealthChecking();
            services.RegisterSwagger();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Authorization");

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<Middleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = HealthHttpResponse.WriteHealthResponse,
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "CoreSwagger");
            });
        }
    }
}
