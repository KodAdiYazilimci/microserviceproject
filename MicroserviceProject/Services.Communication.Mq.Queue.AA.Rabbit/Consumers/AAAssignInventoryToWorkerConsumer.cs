using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Http.Broker.Department.AA.Models;
using Services.Communication.Mq.Queue.AA.Configuration;
using Services.Communication.Mq.Queue.AA.Models;

namespace Services.Communication.Mq.Queue.AA.Rabbit.Consumers
{
    /// <summary>
    /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
    /// </summary>
    public class AAAssignInventoryToWorkerConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<AAWorkerQueueModel> _consumer;

        /// <summary>
        /// İdari işler servis iletişimcisi
        /// </summary>
        private readonly IAACommunicator _aaCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="aaCommunicator">İdari işler servis iletişimcisi</param>
        public AAAssignInventoryToWorkerConsumer(
            IAACommunicator aaCommunicator,
            AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
        {
            _aaCommunicator = aaCommunicator;

            _consumer = new Consumer<AAWorkerQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(AAWorkerQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            await _aaCommunicator.AssignInventoryToWorkerAsync(new AAAssignInventoryToWorkerCommandRequest()
            {
                AssignInventoryToWorkerModels = data.Inventories.Select(x => new AAAssignInventoryToWorkerModel()
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
                    _aaCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
