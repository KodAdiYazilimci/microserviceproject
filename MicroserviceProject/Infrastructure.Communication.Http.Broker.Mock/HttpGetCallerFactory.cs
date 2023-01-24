namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpGetCallerFactory
    {
        private static HttpGetCaller httpGetCaller = null;

        public static HttpGetCaller Instance
        {
            get
            {
                if (httpGetCaller == null)
                {
                    httpGetCaller = new HttpGetCaller();
                }

                return httpGetCaller;
            }
        }
    }
}
