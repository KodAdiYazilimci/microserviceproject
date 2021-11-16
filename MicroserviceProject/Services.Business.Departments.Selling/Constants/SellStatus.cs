namespace Services.Business.Departments.Selling.Constants
{
    /// <summary>
    /// Satış durumları
    /// </summary>
    public enum SellStatus
    {
        /// <summary>
        /// Stok bekliyor
        /// </summary>
        PendingStock = 1,

        /// <summary>
        /// Satışa hazır
        /// </summary>
        ReadyToSell = 2,

        /// <summary>
        /// Üretimi için finans onayı bekliyor
        /// </summary>
        PendingFinanceApprovementToProduce = 3
    }
}
