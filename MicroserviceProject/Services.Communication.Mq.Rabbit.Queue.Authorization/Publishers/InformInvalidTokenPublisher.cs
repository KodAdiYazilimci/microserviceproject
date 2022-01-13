using Services.Communication.Mq.Rabbit.Publisher;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Configuration;
using Services.Communication.Mq.Rabbit.Queue.Authorization.Models;

namespace Services.Communication.Mq.Rabbit.Queue.Authorization.Publishers
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
