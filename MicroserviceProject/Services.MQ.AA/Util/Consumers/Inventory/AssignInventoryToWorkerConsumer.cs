using Communication.Http.Department.AA;
using Communication.Mq.Rabbit.Configuration.Department.AA;
using Communication.Mq.Rabbit.Publisher.Department.AA.Models;

using Infrastructure.Communication.Mq.Rabbit;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.AA.Util.Consumers.Inventory
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
        private readonly Consumer<WorkerModel> _consumer;

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
            AAAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration)
        {
            _aaCommunicator = aaCommunicator;

            _consumer = new Consumer<WorkerModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(WorkerModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.AA.Models.WorkerModel workerModel = new Communication.Http.Department.AA.Models.WorkerModel
            {
                Id = data.Id,
                AAInventories = data.Inventories.Select(x => new Communication.Http.Department.AA.Models.InventoryModel()
                {
                    Id = x.Id,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                }).ToList()
            };

            await _aaCommunicator.AssignInventoryToWorkerAsync(workerModel, cancellationTokenSource);
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
