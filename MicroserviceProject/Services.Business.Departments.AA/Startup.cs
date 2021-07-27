using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Caching.Redis.DI;
using Infrastructure.Communication.Broker.DI;
using Infrastructure.Communication.Http.Broker.Models;
using Infrastructure.Communication.Mq.Rabbit.Configuration.DI;
using Infrastructure.Communication.Mq.Rabbit.Publisher.Department.DI;
using Infrastructure.Localization.DI;
using Infrastructure.Logging.Logger.RequestResponseLogger.DI;
using Infrastructure.Routing.Persistence.DI;
using Infrastructure.Routing.Providers.DI;
using Infrastructure.Security.Authentication.BasicToken.DI;
using Infrastructure.Util.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using Services.Business.Departments.AA.DI;
using Services.UnitOfWork.Sql.DI;

using System.Net;

namespace Services.Business.Departments.AA
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
            services.AddMemoryCache();
            services.RegisterAuthentication();
            services.RegisterBusinessServices();
            services.RegisterRedisCaching();
            services.RegisterCredentialProvider();
            services.RegisterInMemoryCaching();
            services.RegisterLocalizationPersistence();
            services.RegisterLocalizationProviders();
            services.RegisterLogger();
            services.RegisterMappings();
            services.RegisterPublishers();
            services.RegisterQueues();
            services.RegisterRouteProvider();
            services.RegisterRepositories();
            services.RegisterRouteRepositories();
            services.RegisterServiceCommunicator();
            services.RegisterSwagger();
            services.RegisterUnitOfWork();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMiddleware<Middleware>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "CoreSwagger");
            });
        }
    }
}
