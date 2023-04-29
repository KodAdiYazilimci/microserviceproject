
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.ServiceDiscovery.Register.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Authorization.Configuration.Persistence;
using Services.Api.Authorization.DI;
using Services.Communication.Http.Endpoint.Authorization.Endpoints;
using Services.Communication.Mq.Queue.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.Rabbit.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.ServiceDiscovery.DI;
using Services.UnitOfWork.EntityFramework.DI;
using Services.Util.Exception.Handlers;

using System.Collections.Generic;

namespace Services.Api.Authorization
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
            services.RegisterServiceRegisterers();

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

            app.RegisterService(new List<IEndpoint>() { new GetTokenEndpoint(), new GetUserEndpoint(), new HealthCheckEndpoint() });
        }
    }
}
