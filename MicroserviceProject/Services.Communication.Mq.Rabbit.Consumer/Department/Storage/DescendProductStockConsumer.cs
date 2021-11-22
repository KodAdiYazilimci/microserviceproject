using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Storage;
using Services.Communication.Http.Broker.Department.Storage.Models;
using Services.Communication.Mq.Rabbit.Configuration.Department.Storage;
using Services.Communication.Mq.Rabbit.Department.Models.Storage;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Mq.Rabbit.Consumer.Department.Storage
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
        private readonly StorageCommunicator _storageCommunicator;

        /// <summary>
        /// Depolama departmanına bir ürünün stoğunun düşürmeleri tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="storageCommunicator">Depolama departmanı servis iletişimcisi</param>
        public DescendProductStockConsumer(
            DescendProductStockRabbitConfiguration rabbitConfiguration,
            StorageCommunicator storageCommunicator)
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

            await _storageCommunicator.DescendStockAsync(stockModel, cancellationTokenSource);
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
                    _storageCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
