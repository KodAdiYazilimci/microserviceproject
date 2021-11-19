using Communication.Http.Department.Selling;
using Communication.Mq.Rabbit.Configuration.Department.Selling;
using Communication.Mq.Rabbit.Publisher.Department.Finance.Models;

using Infrastructure.Communication.Mq.Rabbit;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.Buying.Util.Consumers.Cost
{
    /// <summary>
    /// Üretilmesi planlanan ürünlere ait onay sonuçlarını tüketen sınıf
    /// </summary>
    public class NotifyProductionRequestApprovementConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ProductionRequestModel> _consumer;

        /// <summary>
        /// Satış departmanı servis iletişimcisi
        /// </summary>
        private readonly SellingCommunicator _sellingCommunicator;

        /// <summary>
        /// Üretilmesi planlanan ürünlere ait onay sonuçlarını tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="sellingCommunicator">Satış departmanı servis iletişimcisi</param>
        public NotifyProductionRequestApprovementConsumer(
            NotifyProductionRequestApprovementRabbitConfiguration rabbitConfiguration,
            SellingCommunicator sellingCommunicator)
        {
            _sellingCommunicator = sellingCommunicator;

            _consumer = new Consumer<ProductionRequestModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProductionRequestModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.Selling.Models.ProductionRequestModel productionRequestModel = new Communication.Http.Department.Selling.Models.ProductionRequestModel
            {
                Approved = data.Approved,
                Amount = data.Amount,
                DepartmentId = data.DepartmentId,
                ProductId = data.ProductId,
                ReferenceNumber = data.ReferenceNumber
            };

            await _sellingCommunicator.NotifyProductionRequest(productionRequestModel, cancellationTokenSource);
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
                    _sellingCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
