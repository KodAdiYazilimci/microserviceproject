using Services.Communication.Http.Broker.Department.Production;
using Services.Communication.Mq.Rabbit.Configuration.Department.Production;
using Services.Communication.Mq.Rabbit.Publisher.Department.Production.Models;

using Infrastructure.Communication.Mq.Rabbit;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.Production.Util.Consumers.Request
{
    /// <summary>
    /// Üretim departmanına üretilmesi istenilen ürünleri tüketen sınıf
    /// </summary>
    public class ProduceConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ProduceModel> _consumer;

        /// <summary>
        /// Üretim departmanı servis iletişimcisi
        /// </summary>
        private readonly ProductionCommunicator _productionCommunicator;

        /// <summary>
        /// Üretim departmanına üretilmesi istenilen ürünleri tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="productionCommunicator">Üretim departmanı servis iletişimcisi</param>
        public ProduceConsumer(
            ProductionProduceRabbitConfiguration rabbitConfiguration,
            ProductionCommunicator productionCommunicator)
        {
            _productionCommunicator = productionCommunicator;

            _consumer = new Consumer<ProduceModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProduceModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _productionCommunicator.ProduceProductAsync(new Services.Communication.Http.Broker.Department.Production.Models.ProduceModel()
            {
                ProductId = data.ProductId,
                Amount = data.Amount,
                DepartmentId = data.DepartmentId,
                ReferenceNumber = data.ReferenceNumber
            }, cancellationTokenSource);
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
                    _productionCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
