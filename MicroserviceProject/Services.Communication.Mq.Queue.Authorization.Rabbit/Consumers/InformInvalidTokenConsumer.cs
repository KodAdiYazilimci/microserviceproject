using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.AA.Abstract;
using Services.Communication.Http.Broker.Department.Accounting.Abstract;
using Services.Communication.Http.Broker.Department.Buying.Abstract;
using Services.Communication.Http.Broker.Department.CR.Abstract;
using Services.Communication.Http.Broker.Department.Finance.Abstract;
using Services.Communication.Http.Broker.Department.HR.Abstract;
using Services.Communication.Http.Broker.Department.IT.Abstract;
using Services.Communication.Http.Broker.Department.Production.Abstract;
using Services.Communication.Http.Broker.Department.Selling.Abstract;
using Services.Communication.Http.Broker.Department.Storage.Abstract;
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
        private readonly IAACommunicator _aaCommunicator;

        private readonly IAccountingCommunicator _accountingCommunicator;

        private readonly IBuyingCommunicator _buyingCommunicator;

        private readonly ICRCommunicator _crCommunicator;

        private readonly IFinanceCommunicator _financeCommunicator;

        private readonly IHRCommunicator _hrCommunicator;

        private readonly IITCommunicator _itCommunicator;

        private readonly IProductionCommunicator _productionCommunicator;

        private readonly ISellingCommunicator _sellingCommunicator;

        private readonly IStorageCommunicator _storageCommunicator;

        /// <summary>
        /// Oturuma ait tokenın artık geçersiz token bilgilerini kuyruktan tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        public InformInvalidTokenConsumer(
            InformInvalidTokenRabbitConfiguration rabbitConfiguration,
            IAACommunicator aaCommunicator,
            IAccountingCommunicator accountingCommunicator,
            IBuyingCommunicator buyingCommunicator,
            ICRCommunicator crCommunicator,
            IFinanceCommunicator financeCommunicator,
            IHRCommunicator hrCommunicator,
            IITCommunicator itCommunicator,
            IProductionCommunicator productionCommunicator,
            ISellingCommunicator sellingCommunicator,
            IStorageCommunicator storageCommunicator)
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
