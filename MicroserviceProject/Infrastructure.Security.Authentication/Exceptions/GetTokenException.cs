using System;

namespace Infrastructure.Security.Authentication.Exceptions
{
    public class GetTokenException : Exception
    {
        public GetTokenException() : base("Token bilgisi alınamadı")
        {

        }

        public GetTokenException(string message) : base(message)
        {

        }
    }
}
