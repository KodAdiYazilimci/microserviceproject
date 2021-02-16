using MicroserviceProject.Services.Model.Department.Accounting;
using MicroserviceProject.Services.Communication.Configuration.Rabbit.Accounting;

namespace MicroserviceProject.Services.Communication.Publishers.Account
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
