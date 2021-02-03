using MicroserviceProject.Model.Logging;
using MicroserviceProject.Services.Security.Authorization.Util.Logging.Loggers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Security.Authorization
{
    public interface IHttpRequestTimeFeature
    {
        DateTime RequestTime { get; }
    }

    public class HttpRequestTimeFeature : IHttpRequestTimeFeature
    {
        public DateTime RequestTime { get; }

        public HttpRequestTimeFeature()
        {
            RequestTime = DateTime.Now;
        }
    }

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Middleware
    {
        private readonly RequestDelegate _next;

        public Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, RequestResponseLogger requestResponseLogger)
        {
            var httpRequestTimeFeature = new HttpRequestTimeFeature();
            httpContext.Features.Set<IHttpRequestTimeFeature>(httpRequestTimeFeature);

            // Start the Timer using Stopwatch  
            var watch = new Stopwatch();
            watch.Start();

            httpContext.Response.OnCompleted(() =>
            {
                watch.Stop();

                requestResponseLogger.Log(new RequestResponseLogModel()
                {
                    ApplicationName = "MicroserviceProject.Services.Security.Authorization",
                    Date = DateTime.Now,
                    Host = httpContext.Request.Host.ToString(),
                    IpAddress = httpContext.Connection.RemoteIpAddress.ToString(),
                    MachineName = Environment.MachineName,
                    Method = httpContext.Request.Method,
                    Protocol = httpContext.Request.Protocol,
                    RequestContentLength = httpContext.Request.ContentLength,
                    ResponseContentLength = httpContext.Response.ContentLength,
                    ResponseContentType = httpContext.Response.ContentType,
                    ResponseTime = watch.ElapsedMilliseconds,
                    StatusCode = httpContext.Response.StatusCode,
                    Url = httpContext.Request.Path.ToString()
                });

                return Task.CompletedTask;
            });

            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Middleware>();
        }
    }
}
