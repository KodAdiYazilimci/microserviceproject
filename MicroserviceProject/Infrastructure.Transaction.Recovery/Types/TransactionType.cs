namespace Infrastructure.Transaction.Recovery
{
    /// <summary>
    /// İşlemin tipi
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Veri setine yeni kayıt eklenerek işlemi tanımlar
        /// </summary>
        Insert = 1,

        /// <summary>
        /// Veri setindeki değer değiştirilerek işlemi tanımlar
        /// </summary>
        Update = 2,

        /// <summary>
        /// Veri setindeki değeri silerek işlemi tanımlar
        /// </summary>
        Delete = 3
    }
}
