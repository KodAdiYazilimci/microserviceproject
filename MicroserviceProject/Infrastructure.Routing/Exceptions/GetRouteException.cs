using System;

namespace Infrastructure.Routing.Exceptions
{
    public class GetRouteException : Exception
    {
        public GetRouteException() : base("Servis rotası bulunamadı")
        {

        }

        public GetRouteException(string message) : base(message)
        {

        }
    }
}
