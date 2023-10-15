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

using Services.Api.Business.Departments.Production.Configuration.Persistence;
using Services.Api.Business.Departments.Production.DI;
using Services.Communication.Http.Broker.Department.Production.DI;
using Services.Communication.Http.Broker.Department.Storage.DI;
using Services.Communication.Http.Endpoint.Department.Production;
using Services.Communication.Mq.Queue.Buying.DI;
using Services.Communication.Mq.Queue.Buying.Rabbit.DI;
using Services.Communication.Mq.Queue.Storage.Configuration.DI;
using Services.Communication.Mq.Queue.Storage.Rabbit.Configuration.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.Aspect.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.ServiceDiscovery.DI;
using Services.UnitOfWork.EntityFramework.DI;
using Services.Util.Exception.Handlers;

using System.Collections.Generic;

namespace Services.Api.Business.Departments.Production
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
            services.RegisterPersistence();
            services.RegisterRepositories();
            services.RegisterValidators();

            services.RegisterBasicTokenAuthentication();
            services.RegisterBuyingQueueConfigurations();
            services.RegisterBuyingQueuePublishers();
            services.RegisterExceptionLogger();
            services.RegisterHttpProductionDepartmentCommunicators();
            services.RegisterLocalizationProviders();
            services.RegisterRequestResponseLogger();
            services.RegisterRuntimeHandlers();
            services.RegisterSqlHealthChecking();
            services.RegisterStorageQueueConfigurations();
            services.RegisterStorageQueuePublishers();
            services.RegisterHttpStorageDepartmentCommunicators();
            services.RegisterSwagger();
            services.RegisterEntityFrameworkUnitOfWork<ProductionContext>();
            services.RegisterServiceRegisterers();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Business.Departments.Production");

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
                new CreateProductEndpoint(),
                new GetProductsEndpoint(),
                new ProduceProductEndpoint(),
                new ReEvaluateProduceProductEndpoint(),
                new RemoveSessionIfExistsInCacheEndpoint(),
                new HealthCheckEndpoint()
            });
        }
    }
}
