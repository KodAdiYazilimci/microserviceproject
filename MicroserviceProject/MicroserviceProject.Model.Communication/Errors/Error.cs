namespace MicroserviceProject.Model.Communication.Errors
{
    /// <summary>
    /// Servisten dönen hata
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Hatanın kodu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Hatanın açıklaması
        /// </summary>
        public string Description { get; set; }
    }
}
