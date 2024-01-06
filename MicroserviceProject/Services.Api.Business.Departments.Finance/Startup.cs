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
using Services.Api.Business.Departments.Finance.DI;
using Services.Business.Departments.Finance.DI;
using Services.Communication.Http.Broker.Department.Finance.DI;
using Services.Communication.Http.Endpoint.Department.Finance;
using Services.Communication.Mq.Queue.Buying.DI;
using Services.Communication.Mq.Queue.Buying.Rabbit.DI;
using Services.Communication.Mq.Queue.Selling.DI;
using Services.Communication.Mq.Queue.Selling.Rabbit.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.Aspect.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.ServiceDiscovery.DI;
using Services.Util.Exception.Handlers;

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
            services.RegisterMappings();
            services.RegisterRepositories();
            services.RegisterValidators();

            services.RegisterBasicTokenAuthentication();
            services.RegisterBuyingQueueConfigurations();
            services.RegisterBuyingQueuePublishers();
            services.RegisterExceptionLogger();
            services.RegisterHttpFinanceDepartmentCommunicators();
            services.RegisterLocalizationProviders();
            services.RegisterRequestResponseLogger();
            services.RegisterRuntimeHandlers();
            services.RegisterSellingQueueConfigurations();
            services.RegisterSellingQueuePublishers();
            services.RegisterSqlHealthChecking();
            services.RegisterSwagger();
            services.RegisterServiceRegisterers();

            services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(Startup).Assembly));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Business.Departments.Finance");

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
                new CreateCostEndpoint(),
                new CreateProductionRequestEndpoint(),
                new DecideCostEndpoint(),
                new GetDecidedCostsEndpoint(),
                new RemoveSessionIfExistsInCacheEndpoint(),
                new HealthCheckEndpoint()
            });
        }
    }
}
