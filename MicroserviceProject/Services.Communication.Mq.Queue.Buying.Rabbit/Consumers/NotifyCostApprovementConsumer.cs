using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Buying.Abstract;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Mq.Queue.Buying.Configuration;
using Services.Communication.Mq.Queue.Buying.Models;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.Consumers
{
    /// <summary>
    /// Satın alınması planlanan envanterlere ait bütçenin sonuçlarını tüketen sınıf
    /// </summary>
    public class NotifyCostApprovementConsumer : IDisposable
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
        /// Satınalma departmanı servis iletişimcisi
        /// </summary>
        private readonly IBuyingCommunicator _buyingCommunicator;

        /// <summary>
        /// Satın alınması planlanan envanterlere ait bütçenin sonuçlarını tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="buyingCommunicator">Satınalma departmanı servis iletişimcisi</param>
        public NotifyCostApprovementConsumer(
            NotifyCostApprovementRabbitConfiguration rabbitConfiguration,
            IBuyingCommunicator buyingCommunicator)
        {
            _buyingCommunicator = buyingCommunicator;

            _consumer = new Consumer<DecidedCostQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(DecidedCostQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.Buying.Models.DecidedCostModel decidedCostModel = new Services.Communication.Http.Broker.Department.Buying.Models.DecidedCostModel
            {
                Approved = data.Approved,
                Done = data.Done,
                InventoryRequestId = data.InventoryRequestId
            };

            await _buyingCommunicator.ValidateCostInventoryAsync(new ValidateCostInventoryCommandRequest()
            {
                DecidedCost = decidedCostModel
            }, data?.TransactionIdentity, cancellationTokenSource);
        }

        /// <summary>
        /// Kayıtları yakalamaya başlar
        /// </summary>
        public async Task StartConsumeAsync(CancellationTokenSource cancellationTokenSource)
        {
            await _consumer.StartConsumeAsync(cancellationTokenSource);
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
                    _buyingCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
