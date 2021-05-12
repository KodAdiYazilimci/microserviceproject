using MicroserviceProject.Infrastructure.Communication.Model.Department.Accounting;
using MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Configuration.Accounting;

using System;

namespace MicroserviceProject.Infrastructure.Communication.Mq.Rabbit.Publisher.Accounting
{
    /// <summary>
    /// Çalışana maaş hesabı açan rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class CreateBankAccountPublisher : BasePublisher<BankAccountModel>, IDisposable
    {
        /// <summary>
        /// Kaynakların serbest bırakılıp bırakılmadığı bilgisi
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Çalışana maaş hesabı açan rabbit kuyruğuna yeni bir kayıt ekler
        /// </summary>
        /// <param name="rabbitConfiguration">Kuyruk ayarlarını verece configuration nesnesi</param>
        public CreateBankAccountPublisher(
            CreateBankAccountRabbitConfiguration rabbitConfiguration)
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
