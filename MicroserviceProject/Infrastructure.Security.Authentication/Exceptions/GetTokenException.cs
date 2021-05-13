using System;

namespace Infrastructure.Security.Authentication.Exceptions
{
    /// <summary>
    /// Yetki tokenı alınamadığında fırlatılacak istisnai durum sınıfı
    /// </summary>
    public class GetTokenException : Exception
    {
        /// <summary>
        /// Yetki tokenı alınamadığında fırlatılacak istisnai durum sınıfı
        /// </summary>
        public GetTokenException() : base("Token bilgisi alınamadı")
        {

        }

        /// <summary>
        /// Yetki tokenı alınamadığında fırlatılacak istisnai durum sınıfı
        /// </summary>
        /// <param name="message">Fırlatılacak mesaj</param>
        public GetTokenException(string message) : base(message)
        {

        }
    }
}
