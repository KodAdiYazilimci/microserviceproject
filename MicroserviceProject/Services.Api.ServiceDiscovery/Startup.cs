using Infrastructure.Caching.Abstraction;
using Infrastructure.Caching.InMemory.DI;
using Infrastructure.Caching.Redis.DI;

using Services.Api.Infrastructure.ServiceDiscovery;
using Services.Api.ServiceDiscovery._DI;
using Services.Api.ServiceDiscovery.DI;
using Services.Api.ServiceDiscovery.DI;
using Services.Logging.Exception.DI;
using Services.Logging.RequestResponse.DI;
using Services.Util.Exception.Handlers;

namespace Services.Api.ServiceDiscovery
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
            services.RegisterRequestResponseLogger();
            services.RegisterExceptionLogger();
            services.RegisterInMemoryCaching();
            services.RegisterRedisCaching();
            services.RegisterSwagger();
            services.RegisterValidators();
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

            app.UseGlobalExceptionHandler(defaultApplicationName: "Services.Api.ServiceDiscovery");

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<Middleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/CoreSwagger/swagger.json", "CoreSwagger");
            });

            app.RegisterService(
                configuration: Configuration,
                distrubutedCacheProvider: app.ApplicationServices.GetRequiredService<IDistrubutedCacheProvider>(),
                inMemoryCacheDataProvider: app.ApplicationServices.GetRequiredService<IInMemoryCacheDataProvider>());
        }
    }
}
