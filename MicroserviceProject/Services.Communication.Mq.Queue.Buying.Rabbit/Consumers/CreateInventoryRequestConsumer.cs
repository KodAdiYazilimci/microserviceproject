using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Buying;
using Services.Communication.Http.Broker.Department.Buying.CQRS.Commands.Requests;
using Services.Communication.Mq.Queue.Buying.Configuration;
using Services.Communication.Mq.Queue.Buying.Models;

namespace Services.Communication.Mq.Queue.Buying.Rabbit.Consumers
{
    /// <summary>
    /// Satınalma departmanına alınması istenilen envanter taleplerini tüketen sınıf
    /// </summary>
    public class CreateInventoryRequestConsumer : IDisposable
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
        /// Satınalma departmanı servis iletişimcisi
        /// </summary>
        private readonly BuyingCommunicator _buyingCommunicator;

        /// <summary>
        /// Satınalma departmanına alınması istenilen envanter taleplerini tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="buyingCommunicator">Satınalma departmanı servis iletişimcisi</param>
        public CreateInventoryRequestConsumer(
            CreateInventoryRequestRabbitConfiguration rabbitConfiguration,
            BuyingCommunicator buyingCommunicator)
        {
            _buyingCommunicator = buyingCommunicator;

            _consumer = new Consumer<InventoryRequestQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(InventoryRequestQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.Buying.Models.InventoryRequestModel inventoryRequestModel =
                new Services.Communication.Http.Broker.Department.Buying.Models.InventoryRequestModel
                {
                    Amount = data.Amount,
                    DepartmentId = data.DepartmentId,
                    InventoryId = data.InventoryId
                };

            await _buyingCommunicator.CreateInventoryRequestAsync(new CreateInventoryRequestCommandRequest()
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
                    _buyingCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
