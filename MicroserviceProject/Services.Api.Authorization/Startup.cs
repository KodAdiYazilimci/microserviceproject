
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Communication.Http.Models;
using Infrastructure.Diagnostics.HealthCheck.Util;
using Infrastructure.Localization.Translation.Provider.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Api.Infrastructure.Authorization.Configuration.Persistence;
using Services.Api.Infrastructure.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.DI;
using Services.Communication.Mq.Queue.Authorization.Rabbit.DI;
using Services.Diagnostics.HealthCheck.DI;
using Services.Logging.RequestResponse.DI;
using Services.UnitOfWork.EntityFramework.DI;

using System;
using System.Collections.Generic;
using System.Net;

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
            services.RegisterInMemoryCaching();
            services.RegisterLocalizationProviders();
            services.RegisterPersistence();
            services.RegisterRepositories();
            services.RegisterRequestResponseLogger();
            services.RegisterSqlHealthChecking(
                connectionStrings: new List<string>() { Configuration.GetSection("Persistence")["DataSource"] });
            services.RegisterSwagger(
                applicationName: Environment.GetEnvironmentVariable("ApplicationName") ?? "Services.Api.Authorization",
                description: "Authorization Api Service");

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
