namespace Infrastructure.ServiceDiscovery.Register.Exceptions
{
    public class ServiceCouldtNotRegisteredToSolidException : Exception
    {
        public ServiceCouldtNotRegisteredToSolidException() : base("Service couldn't registered to solid service")
        {

        }

        public ServiceCouldtNotRegisteredToSolidException(string message) : base(message)
        {

        }
    }
}
