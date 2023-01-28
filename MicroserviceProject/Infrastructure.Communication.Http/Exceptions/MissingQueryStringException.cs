using System;

namespace Infrastructure.Communication.Http.Exceptions
{
    public class MissingQueryStringException : Exception
    {
        public MissingQueryStringException() : base("Belirtilmemiş query string")
        {

        }

        public MissingQueryStringException(string message) : base(message)
        {
        }
    }
}
