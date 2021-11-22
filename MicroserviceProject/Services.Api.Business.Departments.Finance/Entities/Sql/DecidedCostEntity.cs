namespace Services.Business.Departments.Finance.Entities.Sql
{
    /// <summary>
    /// Karar verilen masraflar tablosu entity sınıfı
    /// </summary>
    public class DecidedCostEntity : BaseEntity
    {
        /// <summary>
        /// Envanter talebinin Id değeri
        /// </summary>
        public int InventoryRequestId { get; set; }

        /// <summary>
        /// Masrafın onaylanma durumu
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// Kararın verilip verilmediği bilgisi
        /// </summary>
        public bool Done { get; set; }
    }
}
