namespace Infrastructure.ServiceDiscovery.Exceptions
{
    public class SolidServiceNotDefinedException : Exception
    {
        public SolidServiceNotDefinedException() : base("Solid servis tanımlanmamış")
        {

        }

        public SolidServiceNotDefinedException(string message) : base(message)
        {

        }
    }
}
