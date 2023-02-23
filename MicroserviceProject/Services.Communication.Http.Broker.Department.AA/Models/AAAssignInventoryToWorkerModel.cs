namespace Services.Communication.Http.Broker.Department.AA.Models
{
    public class AAAssignInventoryToWorkerModel
    {
        public int InventoryId { get; set; }
        public int WorkerId { get; set; }
        public int Amount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
