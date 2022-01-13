namespace Services.Communication.Mq.Rabbit.Queue.Authorization.Models
{
    public class InvalidTokenQueueModel : BaseQueueModel
    {
        public string TokenKey { get; set; }
    }
}
