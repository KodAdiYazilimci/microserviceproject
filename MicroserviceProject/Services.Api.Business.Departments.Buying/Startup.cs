using Infrastructure.Communication.Http.Models;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.Util.DI;

using MediatR;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Api.Business.Departments.Buying.DI;
using Services.Communication.Http.Broker.Department.Buying.DI;
using Services.Communication.Mq.Rabbit.Queue.AA.DI;
using Services.Communication.Mq.Rabbit.Queue.Finance.DI;
using Services.Communication.Mq.Rabbit.Queue.IT.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.RequestResponse.DI;
using Services.Security.BasicToken.DI;
using Services.UnitOfWork.Sql.DI;

using System.Collections.Generic;
using System.Net;

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

            services.RegisterAAQueuePublishers();
            services.RegisterBasicTokenAuthentication();
            services.RegisterFinanceQueuePublishers();
            services.RegisterHttpBuyingDepartmentCommunicators();
            services.RegisterITQueuePublishers();
            services.RegisterLocalizationProviders();
            services.RegisterRequestResponseLogger();
            services.RegisterSqlHealthChecking(
                connectionStrings: new List<string>() { Configuration.GetSection("Persistence")["DataSource"] });
            services.RegisterSwagger();
            services.RegisterSqlUnitOfWork();

            services.AddMediatR(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    await
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResultModel()
                    {
                        IsSuccess = false,
                        ErrorModel = new ErrorModel()
                        {
                            Description = context.Features.Get<IExceptionHandlerPathFeature>().Error.Message
                        }
                    }));
                });
            });

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
        }
    }
}
