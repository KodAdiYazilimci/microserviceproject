using Infrastructure.Communication.Broker;
using Infrastructure.Communication.Mq.Rabbit;
using Infrastructure.Communication.Mq.Rabbit.Configuration.Department.Accounting;
using Communication.Mq.Rabbit.Publisher.Department.Accounting.Models;
using Infrastructure.Routing.Providers;

using System;
using System.Threading;
using System.Threading.Tasks;
using Communication.Http.Department.Accounting;
using System.Collections.Generic;
using System.Linq;

namespace Services.MQ.Accounting.Util.Consumers.Inventory
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
        private readonly Consumer<WorkerModel> _consumer;

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

            _consumer = new Consumer<WorkerModel>(rabbitConfiguration);
            _consumer.OnConsumed += Consumer_OnConsumed;
        }

        private async Task Consumer_OnConsumed(WorkerModel data)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Communication.Http.Department.Accounting.Models.WorkerModel workerModel = new Communication.Http.Department.Accounting.Models.WorkerModel
            {
                Id = data.Id,
                BankAccounts = data.BankAccounts.Select(x => new Communication.Http.Department.Accounting.Models.BankAccountModel()
                {
                    IBAN = x.IBAN
                }).ToList()
            };

            await _accountingCommunicator.CreateBankAccountAsync(workerModel, cancellationTokenSource);
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
