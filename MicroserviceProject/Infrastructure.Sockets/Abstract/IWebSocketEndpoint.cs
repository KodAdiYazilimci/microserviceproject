namespace Infrastructure.Sockets.Abstract
{
    public interface IWebSocketEndpoint
    {
        /// <summary>
        /// Soketin Id değeri
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Soketin adı
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Soketin endpointi
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Soketin metod adı
        /// </summary>
        public string Method { get; set; }
    }
}
