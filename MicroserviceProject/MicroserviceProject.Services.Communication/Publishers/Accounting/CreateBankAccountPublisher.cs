using MicroserviceProject.Services.Model.Department.Accounting;
using MicroserviceProject.Services.Communication.Configuration.Rabbit.Accounting;
using System;

namespace MicroserviceProject.Services.Communication.Publishers.Account
{
    /// <summary>
    /// Çalışana maaş hesabı açan rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class CreateBankAccountPublisher : BasePublisher<BankAccountModel>, IDisposable
    {
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
                if (!Disposed)
                {
                    Disposed = true;
                }
            }
        }
    }
}
