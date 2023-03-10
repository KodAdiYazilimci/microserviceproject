
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Security.Authentication.DI;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Presentation.UI.Web.DI;

using Services.Communication.Http.Broker.Authorization.DI;
using Services.Logging.Exception;
using Services.Logging.Exception.Configuration;
using Services.Logging.Exception.DI;
using Services.Security.Cookie.DI;
using Services.Communication.Http.Broker.Gateway.Public.DI;

using System;
using System.Threading;

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
            services.RegisterExceptionLogger();
            services.RegisterHttpAuthorizationCommunicators();
            services.RegisterPublicGatewayCommunicators();
            services.RegisterCookieAuthentication("/Login", "/Yetkisiz");
            services.RegisterInMemoryCaching();
            services.RegisterMappings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    Exception error = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    await app.ApplicationServices.GetRequiredService<ExceptionLogger>().LogAsync(new ExceptionLogModel()
                    {
                        ApplicationName = Environment.GetEnvironmentVariable("ApplicationName") ?? "Presentation.UI.Web",
                        Date = DateTime.UtcNow,
                        MachineName = Environment.MachineName,
                        ExceptionMessage = error.Message,
                        InnerExceptionMessage = error.InnerException?.Message
                    }, new CancellationTokenSource());

                    context.Response.Redirect("/Home/Hata");
                });
            });

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
