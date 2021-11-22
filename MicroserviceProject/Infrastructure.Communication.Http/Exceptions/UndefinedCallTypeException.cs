using System;

namespace Infrastructure.Communication.Http.Exceptions
{
    /// <summary>
    /// Tanımlanmamış çağrı tipi durumunda fırlatılacak istisnai durum
    /// </summary>
    public class UndefinedCallTypeException : Exception
    {
        /// <summary>
        /// Tanımlanmamış çağrı tipi durumunda fırlatılacak istisnai durum
        /// </summary>
        public UndefinedCallTypeException() : base("Belirtilmemiş çağrı tipi")
        {

        }

        /// <summary>
        /// Tanımlanmamış çağrı tipi durumunda fırlatılacak istisnai durum
        /// </summary>
        /// <param name="message">Fırlatılacak mesaj</param>
        public UndefinedCallTypeException(string message) : base(message)
        {

        }
    }
}
