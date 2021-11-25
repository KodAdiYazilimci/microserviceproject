using Hangfire.Common;

using Infrastructure.Communication.Http.Models;
using Infrastructure.Communication.Http.Providers;
using Infrastructure.Diagnostics.HealthCheck.Util.Model;
using Infrastructure.Routing.Models;
using Infrastructure.Routing.Persistence.Repositories.Sql;

using Microsoft.Extensions.Diagnostics.HealthChecks;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Scheduling.Diagnostics.HealthCheck.Jobs
{
    public class CheckApiServicesJob
    {
        private readonly HttpGetProvider _httpGetProvider;
        private readonly HostsRepository _hostsRepository;

        public CheckApiServicesJob(
            HostsRepository hostsRepository,
            HttpGetProvider httpGetProvider)
        {
            _hostsRepository = hostsRepository;
            _httpGetProvider = httpGetProvider;
        }

        public async Task CheckServicesAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            List<HostModel> hosts = await _hostsRepository.GetServiceHostsAsync(cancellationTokenSource);

            foreach (var host in hosts)
            {
                try
                {
                    ServiceResultModel<List<CheckResultModel>> httpResult =
                        await _httpGetProvider.GetAsync<ServiceResultModel<List<CheckResultModel>>>(host.Host + "/health", cancellationTokenSource);

                    if (!httpResult.IsSuccess || httpResult.Data.Any(x => x.Status != HealthStatus.Healthy.ToString()))
                    {

                    }

                    // TODO: Kuyruğa veya web sokete bildirim yapılacak veya loglanacak
                }
                catch (Exception ex)
                {

                }
            }
        }

        public static Job MethodJob
        {
            get
            {
                return new Job(typeof(CheckApiServicesJob).GetMethod(nameof(CheckServicesAsync)));
            }
        }
    }
}
