using MicroserviceProject.Infrastructure.Logging.RabbitMq.Consumers;
using MicroserviceProject.Services.Infrastructure.Logging.Configuration.Logging;
using MicroserviceProject.Services.Logging.Models;
using MicroserviceProject.Services.Logging.Repositories.Sql;

using Microsoft.Extensions.Configuration;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging.Util.Logging.Consumers
{
    /// <summary>
    /// Rabbit sunucusundaki request-response loglarını tüketecek varsayılan sınıf
    /// </summary>
    public class RequestResponseLogConsumer
    {
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
                    new RequestResponseLogRabbitConfiguration(configuration));

            _defaultLogProducer.OnConsumed += _defaultLogProducer_OnConsumed;

            _requestResponseRepository = new RequestResponseLogRepository(configuration);
        }

        public void StartToConsume()
        {
            _defaultLogProducer.StartToConsume();
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

                await _requestResponseRepository.InsertLogAsync(requestResponseLog, cancellationTokenSource.Token);
            }
        }
    }
}
