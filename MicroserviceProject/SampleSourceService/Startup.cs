using Infrastructure.Persistence.ServiceRoutes.Sql.Repositories;

using MicroserviceProject.Infrastructure.Communication.Model.Basics;
using MicroserviceProject.Infrastructure.Communication.Model.Errors;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

using SampleSourceService.Configuration.Services;

using System.Net;

namespace SampleSourceService
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
            services.AddMemoryCache();
            services.AddSingleton(x => 
                    new ServiceRouteRepository(
                        Configuration.GetSection("Configuration").GetSection("Routing").GetSection("DataSource").Value));

            services.RegisterCredentialProvider();
            services.RegisterRouteProvider();
            services.RegisterServiceCommunicator();

            //services.AddSingleton<ServiceCaller>(x =>
            //{
            //    var serviceCaller = new ServiceCaller(x.GetRequiredService<IMemoryCache>(), "1234");

            //    serviceCaller.OnNoServiceFoundInCache += (serviceName) =>
            //    {
            //        var db = new ServiceRouteContext();

            //        var callModel = (from c in db.CallModels
            //                         where c.ServiceName == serviceName
            //                         select
            //                         new CallModel()
            //                         {
            //                             Id = c.Id,
            //                             ServiceName = c.ServiceName,
            //                             Endpoint = c.Endpoint,
            //                             CallType = c.CallType,
            //                             QueryKeys = _serviceRouteContext.QueryKeys.Where(x => x.CallModelId == c.Id).ToList()
            //                         }).FirstOrDefault();

            //        return JsonConvert.SerializeObject(callModel);
            //    };

            //    return serviceCaller;
            //});
            services.AddControllers();

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
                    context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResult()
                    {
                        IsSuccess = false,
                        Error = new Error()
                        {
                            Description = context.Features.Get<IExceptionHandlerPathFeature>().Error.Message
                        }
                    }));
                });
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
