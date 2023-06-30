namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpPutCallerFactory
    {
        public static HttpPutCaller Instance
        {
            get
            {
                return new HttpPutCaller();
            }
        }
    }
}
