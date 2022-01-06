using Infrastructure.Communication.Mq.Rabbit;

using Services.Communication.Http.Broker.Department.Accounting;
using Services.Communication.Mq.Rabbit.Configuration.Department.Accounting;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Communication.Mq.Rabbit.Consumer.Department.Accounting
{
    /// <summary>
    /// Çalışana maaş hesabı açacak kayıtları tüketen sınıf
    /// </summary>
    public class CreateBankAccountConsumer : IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Rabbit kuyruğuyla iletişim kuracak tüketici sınıf
        /// </summary>
        private readonly Consumer<Communication.Mq.Rabbit.Department.Models.Accounting.WorkerQueueModel> _consumer;

        /// <summary>
        /// Muhasebe departmanı servis iletişimcisi
        /// </summary>
        private readonly AccountingCommunicator _accountingCommunicator;

        /// <summary>
        /// Çalışana maaş hesabı açacak kayıtları tüketen sınıf
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarının alınacağın configuration nesnesi</param>
        /// <param name="accountingCommunicator">Muhasebe departmanı servis iletişimcisi</param>
        public CreateBankAccountConsumer(
            CreateBankAccountRabbitConfiguration rabbitConfiguration,
            AccountingCommunicator accountingCommunicator)
        {
            _accountingCommunicator = accountingCommunicator;

            _consumer = new Consumer<Communication.Mq.Rabbit.Department.Models.Accounting.WorkerQueueModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(Communication.Mq.Rabbit.Department.Models.Accounting.WorkerQueueModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Services.Communication.Http.Broker.Department.Accounting.Models.WorkerModel workerModel = new Services.Communication.Http.Broker.Department.Accounting.Models.WorkerModel
            {
                Id = data.Id,
                BankAccounts = data.BankAccounts.Select(x => new Services.Communication.Http.Broker.Department.Accounting.Models.BankAccountModel()
                {
                    IBAN = x.IBAN
                }).ToList()
            };

            await _accountingCommunicator.CreateBankAccountAsync(workerModel, data?.TransactionIdentity, cancellationTokenSource);
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
                    _accountingCommunicator.Dispose();
                }

                disposed = true;
            }
        }
    }
}
