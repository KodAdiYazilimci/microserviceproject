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

using Services.Api.Business.Departments.Selling.Configuration.Persistence;
using Services.Api.Business.Departments.Selling.DI;
using Services.Communication.Http.Broker.Department.Selling.DI;
using Services.Communication.Http.Broker.Department.Storage.DI;
using Services.Communication.Http.Endpoint.Department.Selling;
using Services.Communication.Mq.Queue.Finance.DI;
using Services.Communication.Mq.Queue.Finance.Rabbit.DI;
using Services.Communication.Mq.Queue.Production.DI;
using Services.Communication.Mq.Queue.Production.Rabbit.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.Aspect.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.ServiceDiscovery.DI;
using Services.UnitOfWork.EntityFramework.DI;
using Services.Util.Exception.Handlers;

using System.Collections.Generic;

namespace Services.Api.Business.Departments.Selling
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
            services.RegisterExceptionLogger();
            services.RegisterFinanceQueueConfigurations();
            services.RegisterFinanceQueuePublishers();
            services.RegisterHttpSellingDepartmentCommunicators();
            services.RegisterLocalizationProviders();
            services.RegisterProductionQueueConfigurations();
            services.RegisterProductionQueuePublishers();
            services.RegisterRequestResponseLogger();
            services.RegisterRuntimeHandlers();
            services.RegisterSqlHealthChecking();
            services.RegisterHttpStorageDepartmentCommunicators();
            services.RegisterSwagger();
            services.RegisterEntityFrameworkUnitOfWork<SellingContext>();
            services.RegisterServiceRegisterers();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.Business.Departments.Selling");

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
                new CreateSellingEndpoint(),
                new GetSoldsEndpoint(),
                new NotifyProductionRequestEndpoint(),
                new RemoveSessionIfExistsInCacheEndpoint(),
                new HealthCheckEndpoint()
            });
        }
    }
}
