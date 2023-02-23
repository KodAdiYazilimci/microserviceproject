namespace Services.Communication.Http.Broker.Department.AA.Models
{
    /// <summary>
    /// Çalışanlar
    /// </summary>
    public class AAWorkerModel
    {
        public int Id { get; set; }

        /// <summary>
        /// Başlama tarihi
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Bitiş tarihi
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Çalışanın idari işler envanterleri
        /// </summary>
        public List<AAInventoryModel> Inventories { get; set; }
    }
}
