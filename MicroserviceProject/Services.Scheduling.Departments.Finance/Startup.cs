using Hangfire;
using Hangfire.MemoryStorage;

using Infrastructure.Caching.InMemory.DI;
using Services.Scheduling.Departments.Finance.Jobs;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace Services.Scheduling.Departments.Finance
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(config =>
            {
                config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage();
            });

            services.RegisterInMemoryCaching();

            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseHangfireDashboard();
            app.UseHangfireServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Scheduler is running");
                });
            });

            // 1 defa çalýþtýrýr
            backgroundJobClient.Enqueue<GetExchangeJob>(x => x.CallExchangesAsync());

            // her 1 dakikada bir çalýþtýrýr
            // https://crontab.guru/ 
            recurringJobManager.AddOrUpdate(nameof(GetExchangeJob.CallExchangesAsync), GetExchangeJob.MethodJob, "1 * * * *");
        }
    }
}