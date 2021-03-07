using MicroserviceProject.Services.Infrastructure.Logging.Util.Logging.Loggers;
using MicroserviceProject.Services.Logging.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging
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

        public async Task Invoke(HttpContext httpContext, RequestResponseLogger requestResponseLogger)
        {
            var httpRequestTimeFeature = new HttpRequestTimeFeature();
            httpContext.Features.Set<IHttpRequestTimeFeature>(httpRequestTimeFeature);

            var watch = new Stopwatch();
            watch.Start();

            string response = string.Empty;

            try
            {
                var originalBody = httpContext.Response.Body;
                using (var newBody = new MemoryStream())
                {
                    httpContext.Response.Body = newBody;

                    try
                    {
                        await _next(httpContext);
                    }
                    finally
                    {
                        newBody.Seek(0, SeekOrigin.Begin);
                        response = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
                        newBody.Seek(0, SeekOrigin.Begin);
                        await newBody.CopyToAsync(originalBody);
                    }
                }
            }
            catch (Exception)
            {

            }

            httpContext.Response.OnCompleted(async () =>
            {
                watch.Stop();

                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    await
                    requestResponseLogger.LogAsync(
                        model: new RequestResponseLogModel()
                        {
                            ApplicationName = "MicroserviceProject.Services.Infrastructure.Logging",
                            Content = response,
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
                        },
                        cancellationTokenSource: cancellationTokenSource);
                }
                catch (Exception)
                {

                }
            });
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
