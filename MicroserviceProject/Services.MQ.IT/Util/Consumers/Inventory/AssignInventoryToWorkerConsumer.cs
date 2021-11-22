using Services.Communication.Http.Broker.Department.IT;
using Services.Communication.Mq.Rabbit.Configuration.Department.IT;
using Services.Communication.Mq.Rabbit.Publisher.Department.IT.Models;

using Infrastructure.Communication.Mq.Rabbit;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.MQ.IT.Util.Consumers.Inventory
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
        /// IT departmanı servis iletişimcisi
        /// </summary>
        private readonly ITCommunicator _itCommunicator;

        /// <summary>
        /// Çalışana envanter ataması yapan kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="itCommunicator">IT departmanı servis iletişimcisi</param>
        public AssignInventoryToWorkerConsumer(
            ITAssignInventoryToWorkerRabbitConfiguration rabbitConfiguration,
            ITCommunicator itCommunicator)
        {
            _itCommunicator = itCommunicator;
            _consumer = new Consumer<WorkerModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(WorkerModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.IT.Models.WorkerModel workerModel = new Services.Communication.Http.Broker.Department.IT.Models.WorkerModel
            {
                Id = data.Id,
                ITInventories = data.Inventories.Select(x => new Services.Communication.Http.Broker.Department.IT.Models.InventoryModel
                {
                    Id = x.Id,
                    FromDate = x.FromDate,
                    ToDate = x.ToDate
                }).ToList()
            };

            await _itCommunicator.AssignInventoryToWorkerAsync(workerModel, cancellationTokenSource);
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
