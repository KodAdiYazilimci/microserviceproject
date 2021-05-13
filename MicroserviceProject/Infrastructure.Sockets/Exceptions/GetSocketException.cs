using System;

namespace Infrastructure.Sockets.Exceptions
{
    public class GetSocketException : Exception
    {
        public GetSocketException() : base("Soket bilgisi bulunamadı")
        {

        }

        public GetSocketException(string message) : base(message)
        {

        }
    }
}
