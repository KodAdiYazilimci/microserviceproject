using MicroserviceProject.Services.Business.Model.Department.Accounting;
using MicroserviceProject.Services.Configuration.Communication.Rabbit.Accounting;

namespace MicroserviceProject.Services.Business.Util.Communication.Rabbit.Account
{
    /// <summary>
    /// Çalışana maaş hesabı açan rabbit kuyruğuna yeni bir kayıt ekler
    /// </summary>
    public class CreateBankAccountPublisher : BasePublisher<BankAccountModel>
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
    }
}
