namespace Services.Communication.Mq.Rabbit.Department.Models.Finance
{
    /// <summary>
    /// Envanter satın alım kararı modeli
    /// </summary>
    public class DecidedCostQueueModel : BaseQueueModel
    {
        public int InventoryRequestId { get; set; }
    }
}
