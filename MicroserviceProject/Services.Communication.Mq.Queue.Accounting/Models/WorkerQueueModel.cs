using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Accounting.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        public List<BankAccountQueueModel> BankAccounts { get; set; }
    }
}
