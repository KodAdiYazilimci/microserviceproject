using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.IT.Abstract;
using Services.Communication.Http.Broker.Department.IT.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.IT.Models;
using Services.Communication.Mq.Queue.IT.Configuration;
using Services.Communication.Mq.Queue.IT.Models;

namespace Services.Communication.Mq.Queue.IT.Rabbit.Consumers
{
    /// <summary>
    /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
    /// </summary>
    public class ITAssignInventoryToWorkerConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<ITWorkerQueueModel> _consumer;

        /// <summary>
        /// IT departmanı servis iletişimcisi
        /// </summary>
        private readonly IITCommunicator _itCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="itCommunicator">IT departmanı servis iletişimcisi</param>
        public ITAssignInventoryToWorkerConsumer(
            ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration,
            IITCommunicator itCommunicator)
        {
            _itCommunicator = itCommunicator;
            _consumer = new Consumer<ITWorkerQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(ITWorkerQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _itCommunicator.AssignInventoryToWorkerAsync(new ITAssignInventoryToWorkerCommandRequest()
            {
                AssignInventoryToWorkerModels = data.Inventories.Select(x => new ITAssignInventoryToWorkerModel()
                {
                    InventoryId = x.InventoryId,
                    Amount = x.Amount,
                    WorkerId = x.WorkerId,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                }).ToList()
            }, data.TransactionIdentity, cancellationTokenSource);
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
                    _itCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
