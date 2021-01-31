using Infrastructure.Persistence.InMemory.ServiceRoutes.Configuration;

using MicroserviceProject.Services.Moderator;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Newtonsoft.Json;

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
            services.AddDbContext<ServiceRouteContext>(optionsBuilder =>
            {
                optionsBuilder.UseInMemoryDatabase("ServiceRoutesInMemoryDB");
            });

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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
