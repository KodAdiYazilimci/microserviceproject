using Infrastructure.Communication.Http.Models;
using Infrastructure.Validation.Models;

using System;

namespace Infrastructure.Communication.Http.Exceptions
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
        /// Yanıta konu olan hata
        /// </summary>
        public ErrorModel ErrorModel { get; set; }

        /// <summary>
        /// Yanıta konu olan doğrulama
        /// </summary>
        public ValidationModel Validation { get; set; }

        /// <summary>
        /// Servis işlemleri boyunca geçerli olacak işlem kimliği
        /// </summary>
        public TransactionModel Transaction { get; set; }

        /// <summary>
        /// Servis çağrısı esnasında ortaya çıkan sorunlarda fırlatılacak istisnai durum
        /// </summary>
        /// <param name="message">Duruma ait mesaj</param>
        /// <param name="endpoint">Hatanın ortaya çıktığı endpoint</param>
        public CallException(string message, string endpoint) : base(message)
        {
            this.Endpoint = endpoint;
        }

        /// <summary>
        /// Servis çağrısı esnasında ortaya çıkan sorunlarda fırlatılacak istisnai durum
        /// </summary>
        /// <param name="message">Duruma ait mesaj</param>
        /// <param name="endpoint">Hatanın ortaya çıktığı endpoint</param>
        public CallException(string message, string endpoint, ErrorModel error, ValidationModel validation) : base(message)
        {
            this.Endpoint = endpoint;
            this.Validation = validation;
            this.ErrorModel = error;
        }
    }
}
