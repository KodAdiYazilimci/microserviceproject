
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Presentation.UI.Web.DI;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Communication.Http.Broker.Gateway.DI;
using Services.Security.Cookie.DI;

namespace Presentation.UI.Web
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
            services.AddControllersWithViews();

            services.RegisterCredentialProvider();
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterHttpGatewayCommunicators();
            services.RegisterCookieAuthentication("/Login", "/Yetkisiz");
            services.RegisterInMemoryCaching();
            services.RegisterMappings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Hata");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
