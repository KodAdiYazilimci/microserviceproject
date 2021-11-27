using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Mq.Rabbit.Configuration.Authorization;
using Services.Communication.Mq.Rabbit.Models.Authorization;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Mq.Rabbit.Consumer.Department.AA
{
    /// <summary>
    /// Oturuma ait tokenın artık geçersiz token bilgilerini kuyruktan tüketen sınıf
    /// </summary>
    public class InformInvalidTokenConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<InvalidTokenQueueModel> _consumer;


        /// <summary>
        /// İdari işler servis iletişimcisi
        /// </summary>
        private readonly AACommunicator _aaCommunicator;

        private readonly Services.Communication.Http.Broker.Department.Accounting.AccountingCommunicator accountingCommunicator;

        private readonly Services.Communication.Http.Broker.Department.Buying.BuyingCommunicator buyingCommunicator;

        /// <summary>
        /// Oturuma ait tokenın artık geçersiz token bilgilerini kuyruktan tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        public InformInvalidTokenConsumer(
            InformInvalidTokenRabbitConfiguration rabbitConfiguration)
        {
            _consumer = new Consumer<InvalidTokenQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(InvalidTokenQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
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
                }

                disposed = true;
            }
        }
    }
}
