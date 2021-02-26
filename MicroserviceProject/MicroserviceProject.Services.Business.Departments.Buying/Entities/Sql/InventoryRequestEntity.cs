namespace MicroserviceProject.Services.Business.Departments.Buying.Entities.Sql
{
    /// <summary>
    /// Envanter satın alma talepleri tablosu entity sınıfı
    /// </summary>
    public class InventoryRequestEntity : BaseEntity
    {
        /// <summary>
        /// Satın alınacak envanterin Id si
        /// </summary>
        public int InventoryId { get; set; }

        /// <summary>
        /// Talebi yapan departmanın Id değeri
        /// </summary>
        public int DepartmentId { get; set; }

        /// <summary>
        /// Satın alma miktarı
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// Satın almanın onay durumu
        /// </summary>
        public bool Revoked { get; set; }

        /// <summary>
        /// Satın almanın tamamlanma durumu
        /// </summary>
        public bool Done { get; set; }
    }
}
