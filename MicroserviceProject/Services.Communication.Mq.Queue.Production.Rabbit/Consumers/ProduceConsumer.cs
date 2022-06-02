using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Production;
using Services.Communication.Http.Broker.Department.Production.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.Production.Models;
using Services.Communication.Mq.Queue.Production.Configuration;
using Services.Communication.Mq.Queue.Production.Models;

namespace Services.Communication.Mq.Queue.Production.Rabbit.Consumers
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
        private readonly Consumer<ProduceQueueModel> _consumer;

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

            _consumer = new Consumer<ProduceQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ProduceQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _productionCommunicator.ProduceProductAsync(
                new ProduceProductCommandRequest()
                {
                    Produce = new ProduceModel()
                    {
                        ProductId = data.ProductId,
                        Amount = data.Amount,
                        DepartmentId = data.DepartmentId,
                        ReferenceNumber = data.ReferenceNumber
                    }
                },
                data?.TransactionIdentity,
                cancellationTokenSource);
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
