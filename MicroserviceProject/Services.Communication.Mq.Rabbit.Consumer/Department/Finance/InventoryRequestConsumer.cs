using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Finance;
using Services.Communication.Http.Broker.Department.Finance.Models;
using Services.Communication.Mq.Rabbit.Configuration.Department.Finance;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Mq.Rabbit.Consumer.Department.Finance
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
        private readonly Consumer<DecidedCostModel> _consumer;

        /// <summary>
        /// Finans departmanı servis iletişimcisi
        /// </summary>
        private readonly FinanceCommunicator _financeCommunicator;

        /// <summary>
        /// Satınalma departmanından alınması istenilen envanter taleplerini tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="financeCommunicator">Finans departmanı servis iletişimcisi</param>
        public InventoryRequestConsumer(
            InventoryRequestRabbitConfiguration rabbitConfiguration,
            FinanceCommunicator financeCommunicator)
        {
            _financeCommunicator = financeCommunicator;
            _consumer = new Consumer<DecidedCostModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(DecidedCostModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.Finance.Models.DecidedCostModel decidedCostModel = new Services.Communication.Http.Broker.Department.Finance.Models.DecidedCostModel
            {
                InventoryRequestId = data.InventoryRequestId
            };

            await _financeCommunicator.CreateCostAsync(decidedCostModel, cancellationTokenSource);
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
