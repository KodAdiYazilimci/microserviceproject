using Infrastructure.Communication.Http.Models;
using Infrastructure.Diagnostics.HealthCheck.Util.Model;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Diagnostics.HealthCheck.Util
{
    /// <summary>
    /// Sağlık denetimi sonrasında yazılacak response sınıfı
    /// </summary>
    public class HealthHttpResponse
    {
        /// <summary>
        /// Sağlık sonucunu yazdırır
        /// </summary>
        /// <param name="context">HttpContext nesnesi</param>
        /// <param name="result">HealthReport nesnesi</param>
        /// <returns></returns>
        public static Task WriteHealthResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json; charset=utf-8";

            string json = JsonConvert.SerializeObject(new ServiceResultModel<List<CheckResultModel>>()
            {
                IsSuccess = result.Status == HealthStatus.Healthy,
                Data = result.Entries.Select(x => new CheckResultModel()
                {
                    Status = x.Value.Status.ToString(),
                    Description = x.Value.Description
                }).ToList()
            });

            return context.Response.WriteAsync(json);
        }
    }
}
