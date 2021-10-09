using Communication.Http.Department.Storage;
using Communication.Http.Department.Storage.Models;
using Communication.Mq.Rabbit.Publisher.Department.Storage.Models;

using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Storage;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.Storage.Util.Consumers.ProductStock
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
        private readonly Consumer<ProductStockModel> _consumer;

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

            _consumer = new Consumer<ProductStockModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProductStockModel data)
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
