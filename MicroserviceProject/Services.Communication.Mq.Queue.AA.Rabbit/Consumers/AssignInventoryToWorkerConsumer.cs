using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Http.Broker.Department.AA.CQRS.Commands.Requests;
using Services.Communication.Mq.Queue.AA.Configuration;
using Services.Communication.Mq.Queue.AA.Models;

namespace Services.Communication.Mq.Queue.AA.Rabbit.Consumers
{
    /// <summary>
    /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
    /// </summary>
    public class AssignInventoryToWorkerConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<WorkerQueueModel> _consumer;

        /// <summary>
        /// İdari işler servis iletişimcisi
        /// </summary>
        private readonly AACommunicator _aaCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="aaCommunicator">İdari işler servis iletişimcisi</param>
        public AssignInventoryToWorkerConsumer(
            AACommunicator aaCommunicator,
            AssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
        {
            _aaCommunicator = aaCommunicator;

            _consumer = new Consumer<WorkerQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(WorkerQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.AA.Models.WorkerModel workerModel = new Services.Communication.Http.Broker.Department.AA.Models.WorkerModel
            {
                Id = data.Id,
                AAInventories = data.Inventories.Select(x => new Services.Communication.Http.Broker.Department.AA.Models.InventoryModel()
                {
                    Id = x.Id,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                }).ToList()
            };

            await _aaCommunicator.AssignInventoryToWorkerAsync(new AssignInventoryToWorkerCommandRequest()
            {
                Worker = workerModel
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
                    _aaCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
