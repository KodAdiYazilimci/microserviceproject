using Infrastructure.Caching.Redis.DI;
using Infrastructure.Communication.Http.Broker.DI;
using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.ServiceDiscovery.Register.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Logging.Configuration.Services.Repositories;
using Services.Api.Logging.DI;
using Services.Communication.Http.Endpoint.Logging;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.Exception.DI;
using Services.Security.BasicToken.DI;
using Services.ServiceDiscovery.DI;
using Services.Util.Exception.Handlers;

using System.Collections.Generic;

namespace Services.Api.Logging
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.RegisterBasicTokenAuthentication();
            services.RegisterExceptionLogger();
            services.RegisterRedisCaching();
            services.RegisterLoggers();
            services.RegisterRepositories();
            services.RegisterHttpServiceCommunicator();
            services.RegisterSqlHealthChecking();
            services.RegisterSwagger();
            services.RegisterServiceRegisterers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Logging");

            app.UseRouting();

            app.UseAuthentication();
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

            app.RegisterService(new List<IEndpoint>() { new WriteRequestResponseLogEndpoint() });
        }
    }
}
