namespace MicroserviceProject.Infrastructure.Communication.Moderator.Model.Errors
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
