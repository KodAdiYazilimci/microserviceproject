using Infrastructure.Communication.Http.Endpoint.Abstract;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.ServiceDiscovery.Register.DI;
using Infrastructure.Util.DI;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Services.Api.Business.Departments.Buying.DI;
using Services.Communication.Http.Broker.Department.AA.DI;
using Services.Communication.Http.Broker.Department.Buying.DI;
using Services.Communication.Http.Broker.Department.IT.DI;
using Services.Communication.Http.Endpoint.Department.Buying;
using Services.Communication.Mq.Queue.AA.DI;
using Services.Communication.Mq.Queue.AA.Rabbit.DI;
using Services.Communication.Mq.Queue.Finance.DI;
using Services.Communication.Mq.Queue.Finance.Rabbit.DI;
using Services.Communication.Mq.Queue.IT.DI;
using Services.Communication.Mq.Queue.IT.Rabbit.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.Aspect.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.ServiceDiscovery.DI;
using Services.Util.Exception.Handlers;

using System;
using System.Collections.Generic;

namespace Services.Api.Business.Departments.Buying
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

            services.RegisterBusinessServices();
            services.RegisterRepositories();
            services.RegisterMappings();

            services.RegisterAAQueueConfigurations();
            services.RegisterAAQueuePublishers();
            services.RegisterHttpAADepartmentCommunicators();
            services.RegisterHttpAADepartmentCommunicators();
            services.RegisterBasicTokenAuthentication();
            services.RegisterExceptionLogger();
            services.RegisterFinanceQueueConfigurations();
            services.RegisterFinanceQueuePublishers();
            services.RegisterHttpBuyingDepartmentCommunicators();
            services.RegisterITQueueConfigurations();
            services.RegisterITQueuePublishers();
            services.RegisterHttpITDepartmentCommunicators();
            services.RegisterLocalizationProviders();
            services.RegisterRequestResponseLogger();
            services.RegisterRuntimeHandlers();
            services.RegisterSqlHealthChecking(
                connectionStrings: new List<string>() { Configuration.GetSection("Persistence")["DataSource"] });
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Business.Departments.Buying",
                description: "Buying Api Service");

            services.RegisterServiceRegisterers();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Business.Departments.Buying");

            app.UseMiddleware<Middleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

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

            app.RegisterService(new List<IEndpoint>()
            {
                new CreateInventoryRequestEndpoint(),
                new GetInventoryRequestsEndpoint(),
                new RemoveSessionIfExistsInCacheEndpoint(),
                new ValidateCostInventoryEndpoint(),
                new HealthCheckEndpoint()
            });
        }
    }
}
