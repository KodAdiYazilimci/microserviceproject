using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Selling;
using Services.Communication.Http.Broker.Department.Selling.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Selling.Models;
using Services.Communication.Mq.Rabbit.Queue.Selling.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Selling.Models;

namespace Services.Communication.Mq.Rabbit.Queue.Selling.Consumers
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
        private readonly Consumer<ProductionRequestQueueModel> _consumer;

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

            _consumer = new Consumer<ProductionRequestQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProductionRequestQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            ProductionRequestModel productionRequestModel = new ProductionRequestModel
            {
                Approved = data.Approved,
                Amount = data.Amount,
                DepartmentId = data.DepartmentId,
                ProductId = data.ProductId,
                ReferenceNumber = data.ReferenceNumber
            };

            await _sellingCommunicator.NotifyProductionRequest(
                new NotifyProductionRequestCommandRequest()
                {
                    ProductionRequest = productionRequestModel
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
                    _sellingCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
