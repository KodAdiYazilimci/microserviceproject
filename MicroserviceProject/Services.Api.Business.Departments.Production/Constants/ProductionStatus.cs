namespace Services.Api.Business.Departments.Production.Constants
{
    /// <summary>
    /// Üretim durumları
    /// </summary>
    public enum ProductionStatus
    {
        /// <summary>
        /// Satın alınmayı beklenen alt bağımlı ürünler var
        /// </summary>
        WaitingDependency = 1,

        /// <summary>
        /// Üretime hazır
        /// </summary>
        ReadyToProduce = 2,

        /// <summary>
        /// Üretimi tamamlandı
        /// </summary>
        Completed = 3
    }
}
