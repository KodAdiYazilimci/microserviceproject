using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.IT.Abstract;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Communication.Mq.Queue.IT.Configuration;
using Services.Communication.Mq.Queue.IT.Models;

namespace Services.Communication.Mq.Queue.IT.Rabbit.Consumers
{
    /// <summary>
    /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
    /// </summary>
    public class ITInformInventoryRequestConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ITInventoryRequestQueueModel> _consumer;

        /// <summary>
        /// IT departmanı servis iletişimcisi
        /// </summary>
        private readonly IITCommunicator _itCommunicator;

        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="itCommunicator">IT departmanı servis iletişimcisi</param>
        public ITInformInventoryRequestConsumer(
            InformInventoryRequestRabbitConfiguration rabbitConfiguration,
            IITCommunicator itCommunicator)
        {
            _itCommunicator = itCommunicator;
            _consumer = new Consumer<ITInventoryRequestQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ITInventoryRequestQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ITInventoryRequestModel inventoryRequestModel = new ITInventoryRequestModel
            {
                Amount = data.Amount,
                Done = data.Done,
                InventoryId = data.InventoryId,
                Revoked = data.Revoked
            };

            await _itCommunicator.InformInventoryRequestAsync(new ITInformInventoryRequestCommandRequest()
            {
                InventoryRequest = inventoryRequestModel
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
                    _itCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
