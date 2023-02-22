namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpGetCallerFactory
    {
        public static HttpGetCaller Instance
        {
            get
            {
                return new HttpGetCaller();
            }
        }
    }
}
