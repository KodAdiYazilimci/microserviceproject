using Services.Communication.Mq.Queue.Authorization.Configuration;
using Services.Communication.Mq.Queue.Authorization.Models;
using Services.Communication.Mq.Rabbit.Publisher;

namespace Services.Communication.Mq.Queue.Authorization.Rabbit.Publishers
{
    /// <summary>
    /// Oturuma ait tokenın artık geçersiz olduğunu bildiren kuyruğa kayıt atan sınıf
    /// </summary>
    public class InformInvalidTokenPublisher : BasePublisher<InvalidTokenQueueModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Oturuma ait tokenın artık geçersiz olduğunu bildiren kuyruğa kayıt atan sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public InformInvalidTokenPublisher(
            InformInvalidTokenRabbitConfiguration rabbitConfiguration)
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
