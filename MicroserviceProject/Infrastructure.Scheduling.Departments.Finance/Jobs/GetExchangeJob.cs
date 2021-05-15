using Hangfire.Common;

using Infrastructure.Caching.Redis;
using Infrastructure.Communication.Http.Providers;

using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Scheduling.Departments.Finance.Jobs
{
    public class GetExchangeJob
    {
        private const string STORED_TEMPORARY_EXCHANGES = "stored.temporary.exhanges";

        private readonly RedisCacheDataProvider _redisCacheDataProvider;

        public GetExchangeJob(RedisCacheDataProvider redisCacheDataProvider)
        {
            _redisCacheDataProvider = redisCacheDataProvider;
        }

        public async Task CallExchangesAsync()
        {
            //CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            //HttpGetProvider httpGetProvider = new HttpGetProvider();

            //string result = await httpGetProvider.GetAsync<string>("https://www.tcmb.gov.tr/kurlar/today.xml", cancellationTokenSource);

            await Task.Delay(0);
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
