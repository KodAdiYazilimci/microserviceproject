using Communication.Http.Department.Buying;
using Communication.Mq.Rabbit.Publisher.Department.Buying.Models;

using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Buying;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.Buying.Util.Consumers.Cost
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
        private readonly Consumer<DecidedCostModel> _consumer;

        /// <summary>
        /// Satınalma departmanı servis iletişimcisi
        /// </summary>
        private readonly BuyingCommunicator _buyingCommunicator;

        /// <summary>
        /// Satın alınması planlanan envanterlere ait bütçenin sonuçlarını tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="buyingCommunicator">Satınalma departmanı servis iletişimcisi</param>
        public NotifyCostApprovementConsumer(
            NotifyCostApprovementRabbitConfiguration rabbitConfiguration,
            BuyingCommunicator buyingCommunicator)
        {
            _buyingCommunicator = buyingCommunicator;

            _consumer = new Consumer<DecidedCostModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(DecidedCostModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.Buying.Models.DecidedCostModel decidedCostModel = new Communication.Http.Department.Buying.Models.DecidedCostModel
            {
                Approved = data.Approved,
                Done = data.Done,
                InventoryRequestId = data.InventoryRequestId
            };

            await _buyingCommunicator.ValidateCostInventoryAsync(decidedCostModel, cancellationTokenSource);
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
