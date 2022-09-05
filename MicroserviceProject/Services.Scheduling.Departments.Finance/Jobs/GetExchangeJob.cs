using Hangfire.Common;

using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Providers;

using Services.Scheduling.Departments.Finance.Converters;
using Services.Scheduling.Departments.Finance.Models;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Scheduling.Departments.Finance.Jobs
{
    public class GetExchangeJob
    {
        private const string STORED_LAST_EXCHANGE = "stored.last.exhange";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly InMemoryCacheDataProvider _inMemoryCacheDataProvider;

        public GetExchangeJob(InMemoryCacheDataProvider inMemoryCacheDataProvider, IHttpClientFactory httpClientFactory)
        {
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
            _httpClientFactory = httpClientFactory;
        }

        public async Task CallExchangesAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            HttpGetProvider httpGetProvider = new HttpGetProvider(_httpClientFactory);

            string result = await httpGetProvider.GetAsync("https://www.tcmb.gov.tr/kurlar/today.xml", cancellationTokenSource);

            ExchangeModel exchangeModel = result.ConvertToExchangeModel();

            if (exchangeModel != null
                &&
                (_inMemoryCacheDataProvider.Get<string>(STORED_LAST_EXCHANGE) == null
                ||
                _inMemoryCacheDataProvider.Get<int>(STORED_LAST_EXCHANGE) < Convert.ToInt32(exchangeModel.Bulten.Replace("/", ""))))
            {


                _inMemoryCacheDataProvider.Set<int>(STORED_LAST_EXCHANGE, Convert.ToInt32(exchangeModel.Bulten.Replace("/", "")));
            }
        }

        public static Job MethodJob
        {
            get
            {
                return new Job(typeof(GetExchangeJob).GetMethod(nameof(CallExchangesAsync)));
            }
        }
    }
}
