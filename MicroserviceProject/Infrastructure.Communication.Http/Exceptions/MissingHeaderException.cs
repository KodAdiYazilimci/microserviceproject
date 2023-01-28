using System;

namespace Infrastructure.Communication.Http.Exceptions
{
    public class MissingHeaderException : Exception
    {
        public MissingHeaderException() : base("Belirtilmemiş header")
        {
        }

        public MissingHeaderException(string message) : base(message)
        {

        }
    }
}
