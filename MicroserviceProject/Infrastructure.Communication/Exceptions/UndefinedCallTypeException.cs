using System;

namespace Infrastructure.Communication.Exceptions
{
    public class UndefinedCallTypeException : Exception
    {
        public UndefinedCallTypeException() : base("Belirtilmemiş çağrı tipi")
        {

        }

        public UndefinedCallTypeException(string message) : base(message)
        {

        }
    }
}
