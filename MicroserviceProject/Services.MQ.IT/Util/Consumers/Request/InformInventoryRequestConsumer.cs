using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.IT;
using Services.Communication.Mq.Rabbit.Configuration.Department.IT;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.IT.Util.Consumers.Inventory
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
        private readonly Consumer<Communication.Http.Broker.Department.IT.Models.InventoryRequestModel> _consumer;

        /// <summary>
        /// IT departmanı servis iletişimcisi
        /// </summary>
        private readonly ITCommunicator _itCommunicator;

        /// <summary>
        /// Envanter talebiyle ilgili satınalma sonucunu tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="itCommunicator">IT departmanı servis iletişimcisi</param>
        public InformInventoryRequestConsumer(
            ITInformInventoryRequestRabbitConfiguration rabbitConfiguration,
            ITCommunicator itCommunicator)
        {
            _itCommunicator = itCommunicator;
            _consumer = new Consumer<Communication.Http.Broker.Department.IT.Models.InventoryRequestModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(Communication.Http.Broker.Department.IT.Models.InventoryRequestModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Broker.Department.IT.Models.InventoryRequestModel inventoryRequestModel = new Communication.Http.Broker.Department.IT.Models.InventoryRequestModel
            {
                Amount = data.Amount,
                Done = data.Done,
                InventoryId = data.InventoryId,
                Revoked = data.Revoked
            };

            await _itCommunicator.InformInventoryRequestAsync(inventoryRequestModel, cancellationTokenSource);
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
                    _itCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
