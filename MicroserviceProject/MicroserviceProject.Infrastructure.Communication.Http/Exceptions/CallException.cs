using System;

namespace MicroserviceProject.Infrastructure.Communication.Http.Exceptions
{
    /// <summary>
    /// Servis çağrısı esnasında ortaya çıkan sorunlarda fırlatılacak istisnai durum
    /// </summary>
    public class CallException : Exception
    {
        /// <summary>
        /// Hatanın ortaya çıktığı endpoint
        /// </summary>
        public string Endpoint { get; set; }

        /// <summary>
        /// Servis çağrısı esnasında ortaya çıkan sorunlarda fırlatılacak istisnai durum
        /// </summary>
        /// <param name="message">Duruma ait mesaj</param>
        /// <param name="endpoint">Hatanın ortaya çıktığı endpoint</param>
        public CallException(string message, string endpoint) : base(message)
        {
            this.Endpoint = endpoint;
        }
    }
}
