namespace Infrastructure.Communication.Http.Models
{
    /// <summary>
    /// Http isteğin QueryString parametreleri
    /// </summary>
    public class HttpQuery
    {
        /// <summary>
        /// Sorgunun anahtarı
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Sorgunun değeri
        /// </summary>
        public string Value { get; set; }
    }
}
