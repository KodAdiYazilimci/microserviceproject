using MicroserviceProject.Services.Communication.Configuration.Rabbit.Buying;
using MicroserviceProject.Services.Model.Department.Finance;

using System;

namespace MicroserviceProject.Services.Communication.Publishers.Buying
{
    /// <summary>
    /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekler
    /// </summary>
    public class NotifyCostApprovementPublisher : BasePublisher<DecidedCostModel>, IDisposable
    {
        /// <summary>
        /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public NotifyCostApprovementPublisher(
            NotifyCostApprovementRabbitConfiguration rabbitConfiguration)
            : base(rabbitConfiguration)
        {

        }

        /// <summary>
        /// Kaynakları serbest bırakır
        /// </summary>
        /// <param name="disposing">Kaynakların serbest bırakılıp bırakılmadığı bilgisi</param>
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!Disposed)
                {
                    Disposed = true;
                }
            }
        }
    }
}
