﻿using Infrastructure.Persistence.Logging.Sql.Repositories;

using MicroserviceProject.Infrastructure.Logging.RabbitMq.Consumers;
using MicroserviceProject.Model.Logging;
using MicroserviceProject.Services.Infrastructure.Logging.Logging.Configuration;

using Microsoft.Extensions.Configuration;

using System.Threading;
using System.Threading.Tasks;

namespace MicroserviceProject.Services.Infrastructure.Logging.Logging.Consumers
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

        private readonly RequestResponseRepository _requestResponseRepository;

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

            _requestResponseRepository = new RequestResponseRepository(GetConnectionString(configuration));

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

        /// <summary>
        /// Logların yazılacağı veritabanı bağlantı cümlesini verir
        /// </summary>
        /// <param name="configuration">Veritabanı bağlantı cümlesini getirecek configuration</param>
        /// <returns></returns>
        private string GetConnectionString(IConfiguration configuration)
        {
            string connectionString =
                configuration
                .GetSection("Configuration")
                .GetSection("Logging")
                .GetSection("RequestResponseLogging")
                .GetSection("RabbitConfiguration")
                .GetSection("RequestResponseLogging")
                .GetSection("DataSource").Value;

            return connectionString;

        }
    }
}
