namespace Services.Business.Departments.Finance.Entities.Sql
{
    /// <summary>
    /// Üretilecek ürünlerin üretim talebi tablosu entity sınıfı
    /// </summary>
    public class ProductionRequestEntity : BaseEntity
    {
        /// <summary>
        /// Üretilmesi istenilen ürünün Id değeri
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Üretilmesi istenilen miktar
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Üretim talebinde bulunan departmanın Id değeri
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Referans numarası
        /// </summary>
        public int ReferenceNumber { get; set; }

        /// <summary>
        /// Üretimin onaylanma durumu
        /// </summary>
        public bool Approved { get; set; }

        /// <summary>
        /// Kararın verilip verilmediği bilgisi
        /// </summary>
        public bool Done { get; set; }
    }
}
