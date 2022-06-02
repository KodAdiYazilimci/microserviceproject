using Services.Communication.Mq.Models;

namespace Services.Communication.Mq.Queue.Authorization.Models
{
    public class InvalidTokenQueueModel : BaseQueueModel
    {
        public string TokenKey { get; set; }
    }
}
