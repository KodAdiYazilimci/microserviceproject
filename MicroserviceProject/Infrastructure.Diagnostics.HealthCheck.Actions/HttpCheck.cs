using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Providers;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Diagnostics.HealthCheck.Actions
{
    /// <summary>
    /// Http isteklerinin sağlıklı çalıştığını kontrol eden sınıf
    /// </summary>
    public class HttpCheck : IHealthCheck
    {
        /// <summary>
        /// Denetlenecek adresler
        /// </summary>
        private readonly List<string> urls;

        /// <summary>
        /// Http isteklerinin sağlıklı çalıştığını kontrol eden sınıf
        /// </summary>
        /// <param name="urls">Denetlenecek adresler</param>
        public HttpCheck(List<string> urls)
        {
            this.urls = urls;
        }

        /// <summary>
        /// Http isteklerinin sağlıklı çalıştığını kontrol eder
        /// </summary>
        /// <param name="context">HealthCheckContext nesnesi</param>
        /// <param name="cancellationToken">İptal tokenı</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (HttpGetProvider httpGetProvider = new HttpGetProvider())
            {
                try
                {
                    foreach (var url in urls)
                    {
                        string getResultAsJson = await httpGetProvider.GetAsync(url);

                        ServiceResultModel serviceResult = JsonConvert.DeserializeObject<ServiceResultModel>(getResultAsJson);

                        if (serviceResult != null && !serviceResult.IsSuccess)
                        {
                            return new HealthCheckResult(HealthStatus.Unhealthy, description: serviceResult.ErrorModel?.Description);
                        }
                    }

                    return HealthCheckResult.Healthy("Http requests are successed");
                }
                catch (Exception ex)
                {
                    return new HealthCheckResult(HealthStatus.Unhealthy, exception: ex, description: "Http request failed");
                }
            }
        }
    }
}
