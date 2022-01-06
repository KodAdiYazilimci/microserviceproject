namespace Services.Communication.Mq.Rabbit.Models.Authorization
{
    public class InvalidTokenQueueModel : BaseQueueModel
    {
        public string TokenKey { get; set; }
    }
}
