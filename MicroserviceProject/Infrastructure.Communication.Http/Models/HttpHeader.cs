namespace Infrastructure.Communication.Http.Models
{
    /// <summary>
    /// Http isteğinin headerı
    /// </summary>
    public class HttpHeader
    {
        /// <summary>
        /// Http isteğinin headerı
        /// </summary>
        public HttpHeader()
        {

        }

        /// <summary>
        /// Http isteğinin headerı
        /// </summary>
        /// <param name="key">Headerın anahtarı</param>
        /// <param name="value">Headerın değeri</param>
        public HttpHeader(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

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
