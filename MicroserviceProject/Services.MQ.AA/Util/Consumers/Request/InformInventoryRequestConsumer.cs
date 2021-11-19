using Communication.Http.Department.AA;
using Communication.Mq.Rabbit.Configuration.Department.AA;
using Communication.Mq.Rabbit.Publisher.Department.AA.Models;

using Infrastructure.Communication.Mq.Rabbit;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.AA.Util.Consumers.Request
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
        private readonly Consumer<InventoryRequestModel> _consumer;

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
            AAInformInventoryRequestRabbitConfiguration rabbitConfiguration,
            AACommunicator aaCommunicator)
        {
            _aaCommunicator = aaCommunicator;

            _consumer = new Consumer<InventoryRequestModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(InventoryRequestModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.AA.Models.InventoryRequestModel inventoryRequestModel = new Communication.Http.Department.AA.Models.InventoryRequestModel
            {
                Amount = data.Amount,
                Done = data.Done,
                InventoryId = data.InventoryId,
                Revoked = data.Revoked
            };

            await _aaCommunicator.InformInventoryRequestAsync(inventoryRequestModel, cancellationTokenSource);
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
