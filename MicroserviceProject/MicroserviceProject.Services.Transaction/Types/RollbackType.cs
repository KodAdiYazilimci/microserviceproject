namespace MicroserviceProject.Services.Transaction.Types
{
    /// <summary>
    /// İşlemin geri alınma şekli
    /// </summary>
    public enum RollbackType
    {
        /// <summary>
        /// Veri setine yeni kayıt eklenerek işlem geri alınır
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Veri setindeki değer değiştirilerek işlem geri alınır
        /// </summary>
        Update = 2,

        /// <summary>
        /// Veri setindeki değer silinerek işlem geri alınır
        /// </summary>
        Delete = 3,

        /// <summary>
        /// Veri setindeki değeri artırır
        /// </summary>
        IncreaseValue = 4,

        /// <summary>
        /// Veri setindeki değeri azaltır
        /// </summary>
        DecreaseValue = 5
    }
}
