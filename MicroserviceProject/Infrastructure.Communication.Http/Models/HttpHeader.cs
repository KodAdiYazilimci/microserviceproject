namespace MicroserviceProject.Infrastructure.Communication.Http.Models
{
    /// <summary>
    /// Http isteğinin header ı
    /// </summary>
    public class HttpHeader
    {
        /// <summary>
        /// Headerın anahtarı
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Headerın değeri
        /// </summary>
        public string Value { get; set; }
    }
}
