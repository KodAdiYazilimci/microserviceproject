using Infrastructure.Communication.Mq.Rabbit.Configuration.Buying;
using Services.Model.Department.Finance;

using System;

namespace Infrastructure.Communication.Mq.Rabbit.Publisher.Buying
{
    /// <summary>
    /// Satın alınması istenilen envanterlere ait kararları rabbit kuyruğuna ekler
    /// </summary>
    public class NotifyCostApprovementPublisher : BasePublisher<DecidedCostModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

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
                if (!disposed)
                {
                    disposed = true;
                }
            }
        }
    }
}
