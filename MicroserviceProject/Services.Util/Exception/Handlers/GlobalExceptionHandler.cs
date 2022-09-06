using Infrastructure.Communication.Http.Models;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using Services.Logging.Exception;
using Services.Logging.Exception.Configuration;

using System.Net;

namespace Services.Util.Exception.Handlers
{
    /// <summary>
    /// Uygulama bazlı exception handling sınıfı
    /// </summary>
    public static class GlobalExceptionHandler
    {
        /// <summary>
        /// Uygulama bazında genel exception olaylarını handle eder
        /// </summary>
        /// <param name="applicationBuilder">IApplicationBuilder nesnesi</param>
        /// <param name="defaultApplicationName">Uygulamanın varsayılan adı</param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder applicationBuilder, string defaultApplicationName)
        {
            applicationBuilder.UseExceptionHandler(handler =>
            {
                handler.Run(context =>
                {
                    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                    System.Exception error = context.Features.Get<IExceptionHandlerPathFeature>().Error;

                    Task logTask = applicationBuilder.ApplicationServices.GetRequiredService<ExceptionLogger>().LogAsync(new ExceptionLogModel()
                    {
                        ApplicationName = Environment.GetEnvironmentVariable("ApplicationName") ?? defaultApplicationName,
                        Date = DateTime.UtcNow,
                        MachineName = Environment.MachineName,
                        ExceptionMessage = error.Message,
                        InnerExceptionMessage = error.InnerException?.Message
                    }, cancellationTokenSource);

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";

                    Task responseTask = context.Response.WriteAsync(JsonConvert.SerializeObject(new ServiceResultModel()
                    {
                        IsSuccess = false,
                        ErrorModel = new ErrorModel()
                        {
                            Description =
                            error.Message
                            +
                            (error.InnerException != null ? (Environment.NewLine + error.InnerException.Message) : String.Empty)
                        }
                    }), cancellationTokenSource.Token);

                    Task.WaitAll(new Task[] { logTask, responseTask }, cancellationTokenSource.Token);

                    return Task.CompletedTask;
                });
            });

            return applicationBuilder;
        }
    }
}
