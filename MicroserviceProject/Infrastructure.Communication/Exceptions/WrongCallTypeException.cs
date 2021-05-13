using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Communication.Exceptions
{
    public class WrongCallTypeException : Exception
    {
        public WrongCallTypeException() : base("Hatalı çağrı tipi")
        {

        }

        public WrongCallTypeException(string message) : base(message)
        {

        }
    }
}
