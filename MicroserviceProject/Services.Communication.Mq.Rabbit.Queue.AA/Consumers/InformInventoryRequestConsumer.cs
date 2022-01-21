using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Mq.Rabbit.Queue.AA.Configuration;
using Services.Communication.Mq.Rabbit.Queue.AA.Models;

namespace Services.Communication.Mq.Rabbit.Queue.AA.Consumers
{
    /// <summary>
    /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
    /// </summary>
    public class InformInventoryRequestConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<InventoryRequestQueueModel> _consumer;

        /// <summary>
        /// İdari işler servis iletişimcisi
        /// </summary>
        private readonly AACommunicator _aaCommunicator;

        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="aaCommunicator">İdari işler servis iletişimcisi</param>
        public InformInventoryRequestConsumer(
            InformInventoryRequestRabbitConfiguration rabbitConfiguration,
            AACommunicator aaCommunicator)
        {
            _aaCommunicator = aaCommunicator;

            _consumer = new Consumer<InventoryRequestQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(InventoryRequestQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.AA.Models.InventoryRequestModel inventoryRequestModel = new Services.Communication.Http.Broker.Department.AA.Models.InventoryRequestModel
            {
                Amount = data.Amount,
                Done = data.Done,
                InventoryId = data.InventoryId,
                Revoked = data.Revoked
            };

            await _aaCommunicator.InformInventoryRequestAsync(new InformInventoryRequestCommandRequest()
            {
                InventoryRequest = inventoryRequestModel
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
                    _aaCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
