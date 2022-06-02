using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA;
using Services.Communication.Http.Broker.Department.Accounting;
using Services.Communication.Http.Broker.Department.Buying;
using Services.Communication.Http.Broker.Department.CR;
using Services.Communication.Http.Broker.Department.Finance;
using Services.Communication.Http.Broker.Department.HR;
using Services.Communication.Http.Broker.Department.IT;
using Services.Communication.Http.Broker.Department.Production;
using Services.Communication.Http.Broker.Department.Selling;
using Services.Communication.Http.Broker.Department.Storage;
using Services.Communication.Mq.Queue.Authorization.Configuration;
using Services.Communication.Mq.Queue.Authorization.Models;

namespace Services.Communication.Mq.Queue.Authorization.Rabbit.Consumers
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

        private readonly AccountingCommunicator _accountingCommunicator;

        private readonly BuyingCommunicator _buyingCommunicator;

        private readonly CRCommunicator _crCommunicator;

        private readonly FinanceCommunicator _financeCommunicator;

        private readonly HRCommunicator _hrCommunicator;

        private readonly ITCommunicator _itCommunicator;

        private readonly ProductionCommunicator _productionCommunicator;

        private readonly SellingCommunicator _sellingCommunicator;

        private readonly StorageCommunicator _storageCommunicator;

        /// <summary>
        /// Oturuma ait tokenın artık geçersiz token bilgilerini kuyruktan tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        public InformInvalidTokenConsumer(
            InformInvalidTokenRabbitConfiguration rabbitConfiguration,
            AACommunicator aaCommunicator,
            AccountingCommunicator accountingCommunicator,
            BuyingCommunicator buyingCommunicator,
            CRCommunicator crCommunicator,
            FinanceCommunicator financeCommunicator,
            HRCommunicator hrCommunicator,
            ITCommunicator itCommunicator,
            ProductionCommunicator productionCommunicator,
            SellingCommunicator sellingCommunicator,
            StorageCommunicator storageCommunicator)
        {
            _consumer = new Consumer<InvalidTokenQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
            _aaCommunicator = aaCommunicator;
            _accountingCommunicator = accountingCommunicator;
            _buyingCommunicator = buyingCommunicator;
            _crCommunicator = crCommunicator;
            _financeCommunicator = financeCommunicator;
            _hrCommunicator = hrCommunicator;
            _itCommunicator = itCommunicator;
            _productionCommunicator = productionCommunicator;
            _sellingCommunicator = sellingCommunicator;
            _storageCommunicator = storageCommunicator;
        }

        private Task Consumer_OnConsumed(InvalidTokenQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.WaitAll(
                _aaCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _accountingCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _buyingCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _crCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _financeCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _hrCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _itCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _productionCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _sellingCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource),
                _storageCommunicator.RemoveSessionIfExistsInCacheAsync(data.TokenKey, cancellationTokenSource));

            return Task.CompletedTask;
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
