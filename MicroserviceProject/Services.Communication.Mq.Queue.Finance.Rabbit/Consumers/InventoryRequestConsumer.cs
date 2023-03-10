using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Finance.Abstract;
using Services.Communication.Http.Broker.Department.Finance.Models;
using Services.Communication.Mq.Queue.Finance.Configuration;
using Services.Communication.Mq.Queue.Finance.Models;

namespace Services.Communication.Mq.Queue.Finance.Rabbit.Consumers
{
    /// <summary>
    /// Satınalma departmanından alınması istenilen envanter taleplerini tüketen sınıf
    /// </summary>
    public class InventoryRequestConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<DecidedCostQueueModel> _consumer;

        /// <summary>
        /// Finans departmanı servis iletişimcisi
        /// </summary>
        private readonly IFinanceCommunicator _financeCommunicator;

        /// <summary>
        /// Satınalma departmanından alınması istenilen envanter taleplerini tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="financeCommunicator">Finans departmanı servis iletişimcisi</param>
        public InventoryRequestConsumer(
            InventoryRequestRabbitConfiguration rabbitConfiguration,
            IFinanceCommunicator financeCommunicator)
        {
            _financeCommunicator = financeCommunicator;
            _consumer = new Consumer<DecidedCostQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(DecidedCostQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            DecidedCostModel decidedCostModel = new DecidedCostModel
            {
                InventoryRequestId = data.InventoryRequestId
            };

            await _financeCommunicator.CreateCostAsync(new Http.Broker.Department.Finance.CQRS.Commands.Requests.CreateCostCommandRequest()
            {
                Cost = decidedCostModel
            }, data?.TransactionIdentity, cancellationTokenSource);
        }

        /// <summary>
        /// Kayıtları yakalamaya başlar
        /// </summary>
        public void StartToConsume()
        {
            _consumer.StartToConsume();
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
                    _consumer.Dispose();
                    _financeCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
