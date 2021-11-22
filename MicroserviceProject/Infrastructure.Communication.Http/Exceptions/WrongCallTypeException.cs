using System;

namespace Infrastructure.Communication.Http.Exceptions
{
    /// <summary>
    /// Yanlış çağrı tipi durumunda fırlatılacak istisnai durum
    /// </summary>
    public class WrongCallTypeException : Exception
    {
        /// <summary>
        /// Yanlış çağrı tipi durumunda fırlatılacak istisnai durum
        /// </summary>
        public WrongCallTypeException() : base("Hatalı çağrı tipi")
        {

        }

        /// <summary>
        /// Yanlış çağrı tipi durumunda fırlatılacak istisnai durum
        /// </summary>
        /// <param name="message">Fırlatılacak mesaj</param>
        public WrongCallTypeException(string message) : base(message)
        {

        }
    }
}
