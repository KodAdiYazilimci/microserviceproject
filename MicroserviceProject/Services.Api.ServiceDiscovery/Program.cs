using Infrastructure.Caching.Abstraction;
using Infrastructure.Caching.Redis.DI;
using Infrastructure.ServiceDiscovery.Models;

using Newtonsoft.Json;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.RegisterRedisCaching();
        // Add services to the container.

        var app = builder.Build();

        // Configure the HTTP request pipeline.


        app.MapGet("/", (httpContext) =>
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.WriteAsync("Service Up!");

            return Task.CompletedTask;
        });

        app.MapPost("/Register", async (HttpContext httpContext) =>
        {
            using (StreamReader streamReader = new StreamReader(httpContext.Request.Body))
            {
                string json = await streamReader.ReadToEndAsync();

                ServiceModel serviceModel = JsonConvert.DeserializeObject<ServiceModel>(json);

                if (serviceModel != null && !string.IsNullOrWhiteSpace(serviceModel.ServiceName))
                {
                    IDistrubutedCacheProvider cacheProvider = app.Services.GetRequiredService<IDistrubutedCacheProvider>();

                    Dictionary<string, ServiceModel>? services = null;

                    if (cacheProvider.TryGetValue("Cached_Services", out string cachedJson) && !string.IsNullOrEmpty(cachedJson))
                    {
                        services = JsonConvert.DeserializeObject<Dictionary<string, ServiceModel>>(cachedJson);
                    }
                    else
                    {
                        services = new Dictionary<string, ServiceModel>();
                    }

                    services.Add(serviceModel.ServiceName, serviceModel);

                    cacheProvider.Set<Dictionary<string, ServiceModel>>("Cached_Services", services, DateTime.UtcNow.AddYears(1));
                }
                else
                    return Results.BadRequest();
            }

            return Results.Ok();
        });

        app.MapGet("/Discover", () =>
        {
            IDistrubutedCacheProvider cacheProvider = app.Services.GetRequiredService<IDistrubutedCacheProvider>();

            if (cacheProvider.TryGetValue("Cached_Services", out Dictionary<string, ServiceModel> services))
            {
                return services;
            }

            return new Dictionary<string, ServiceModel>();
        });

        app.Run();
    }
}