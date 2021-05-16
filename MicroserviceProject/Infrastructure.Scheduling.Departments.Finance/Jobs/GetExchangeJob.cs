using Hangfire.Common;

using Infrastructure.Caching.InMemory;
using Infrastructure.Communication.Http.Providers;
using Infrastructure.Scheduling.Departments.Finance.Converters;
using Infrastructure.Scheduling.Departments.Finance.Models;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Scheduling.Departments.Finance.Jobs
{
    public class GetExchangeJob
    {
        private const string STORED_LAST_EXCHANGE = "stored.last.exhange";

        private readonly InMemoryCacheDataProvider _inMemoryCacheDataProvider;

        public GetExchangeJob(InMemoryCacheDataProvider inMemoryCacheDataProvider)
        {
            _inMemoryCacheDataProvider = inMemoryCacheDataProvider;
        }

        public async Task CallExchangesAsync()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            HttpGetProvider httpGetProvider = new HttpGetProvider();

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
