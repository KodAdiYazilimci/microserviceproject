namespace Services.Business.Departments.IT.Entities.Sql
{
    /// <summary>
    /// Her bir öğe için geri alma entity sınıfı
    /// </summary>
    public class RollbackItemEntity : BaseEntity
    {

        /// <summary>
        /// İşlem öğesinin üst kimliği
        /// </summary>
        public string TransactionIdentity { get; set; }

        /// <summary>
        /// Öğenin geri alma şekli
        /// </summary>
        public int RollbackType { get; set; }

        /// <summary>
        /// Öğenin bulunduğu veri seti
        /// </summary>
        public object DataSet { get; set; }

        /// <summary>
        /// İşlem öğesinin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Öğenin tanımlayıcısı
        /// </summary>
        public object Identity { get; set; }

        /// <summary>
        /// Öğenin önceki değeri
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// Öğenin yeni değeri
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// İşlem öğesinin geri alınıp alınmadığı bilgisi
        /// </summary>
        public bool IsRolledback { get; set; }
    }
}
