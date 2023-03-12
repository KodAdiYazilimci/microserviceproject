using Infrastructure.Logging.RabbitMq.Consumers;

using Microsoft.Extensions.Configuration;

using Services.Logging.RequestResponse.Configuration;
using Services.Logging.RequestResponse.Persistence;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Api.Infrastructure.Logging.Util.Logging.Consumers
{
    /// <summary>
    /// Rabbit sunucusundaki request-response loglarını tüketecek varsayılan sınıf
    /// </summary>
    public class RequestResponseLogConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Request-response logları çekebilmek için rabbit sunucusunun yapılandırma ayarları
        /// </summary>
        private readonly DefaultLogConsumer<RequestResponseLogModel> _defaultLogProducer;

        private readonly RequestResponseLogRepository _requestResponseRepository;

        /// <summary>
        /// Rabbit sunucusuna request-response log üretecek varsayılan sınıf
        /// </summary>
        /// <param name="configuration">Request-response logları çekebilmek için rabbit sunucusunun yapılandırma ayarları</param>
        public RequestResponseLogConsumer(IConfiguration configuration)
        {
            _defaultLogProducer =
                new DefaultLogConsumer<RequestResponseLogModel>(
                    new Configuration.Logging.RequestResponseLogRabbitConfiguration(configuration));

            _defaultLogProducer.OnConsumed += _defaultLogProducer_OnConsumed;

            _requestResponseRepository = new RequestResponseLogRepository(configuration);
        }

        public async Task StartConsumeAsync(CancellationTokenSource cancellationTokenSource)
        {
            await _defaultLogProducer.StartConsumeAsync(cancellationTokenSource);
        }

        /// <summary>
        /// Log consume edildiğinde ateşlenecek olayın metodu
        /// </summary>
        /// <param name="requestResponseLog">Rabbit sunucusundan yakalanan log nesnesi</param>
        /// <returns></returns>
        private async Task _defaultLogProducer_OnConsumed(RequestResponseLogModel requestResponseLog)
        {
            if (requestResponseLog != null)
            {
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                await _requestResponseRepository.InsertLogAsync(requestResponseLog, cancellationTokenSource);
            }
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    _defaultLogProducer.Dispose();
                    _requestResponseRepository.Dispose();
                }

                disposed = true;
            }
        }
    }
}
