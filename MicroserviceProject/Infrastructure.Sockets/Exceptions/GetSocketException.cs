using System;

namespace Infrastructure.Sockets.Exceptions
{
    /// <summary>
    /// Soket bilgisi bulunamadığında fırlatılacak istisnai durum sınıfı
    /// </summary>
    public class GetSocketException : Exception
    {
        /// <summary>
        /// Soket bilgisi bulunamadığında fırlatılacak istisnai durum sınıfı
        /// </summary>
        public GetSocketException() : base("Soket bilgisi bulunamadı")
        {

        }

        /// <summary>
        /// Soket bilgisi bulunamadığında fırlatılacak istisnai durum sınıfı
        /// </summary>
        /// <param name="message">Fırlatılacak mesaj</param>
        public GetSocketException(string message) : base(message)
        {

        }
    }
}
