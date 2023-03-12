using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Storage.Abstract;
using Services.Communication.Http.Broker.Department.Storage.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Storage.Models;
using Services.Communication.Mq.Queue.Storage.Configuration;
using Services.Communication.Mq.Queue.Storage.Models;

namespace Services.Communication.Mq.Queue.Storage.Rabbit.Consumers
{
    /// <summary>
    /// Depolama departmanına bir ürünün stoğunun düşürmeleri tüketen sınıf
    /// </summary>
    public class DescendProductStockConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ProductStockQueueModel> _consumer;

        /// <summary>
        /// Depolama departmanı servis iletişimcisi
        /// </summary>
        private readonly IStorageCommunicator _storageCommunicator;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun düşürmeleri tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="storageCommunicator">Depolama departmanı servis iletişimcisi</param>
        public DescendProductStockConsumer(
            DescendProductStockRabbitConfiguration rabbitConfiguration,
            IStorageCommunicator storageCommunicator)
        {
            _storageCommunicator = storageCommunicator;

            _consumer = new Consumer<ProductStockQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProductStockQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            StockModel stockModel = new StockModel();

            stockModel.Amount = data.Amount;
            stockModel.ProductId = data.ProductId;

            await _storageCommunicator.DescendStockAsync(new DescendProductStockCommandRequest()
            {
                Stock = stockModel,
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
                    _storageCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
