namespace Services.Communication.Mq.Rabbit.Queue.AA.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class WorkerQueueModel : BaseQueueModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Çalışanın envanterleri
        /// </summary>
        public List<InventoryQueueModel> Inventories { get; set; }
    }
}
