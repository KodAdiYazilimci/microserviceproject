namespace Infrastructure.Diagnostics.HealthCheck.Util.Model
{
    /// <summary>
    /// Sağlık denetimi modeli
    /// </summary>
    public class CheckResultModel
    {
        /// <summary>
        /// Kontrolün sonucu
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Kontrole dair açıklama
        /// </summary>
        public string Description { get; set; }
    }
}
