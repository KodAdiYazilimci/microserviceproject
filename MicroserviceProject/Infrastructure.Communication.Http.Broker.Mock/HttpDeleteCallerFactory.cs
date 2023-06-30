namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpDeleteCallerFactory
    {
        public static HttpDeleteCaller Instance
        {
            get
            {
                return new HttpDeleteCaller();
            }
        }
    }
}
