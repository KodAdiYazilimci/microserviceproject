namespace Infrastructure.Communication.Http.Broker.Mock
{
    public class HttpPostCallerFactory
    {
        private static HttpPostCaller httpPostCaller = null;

        public static HttpPostCaller Instance
        {
            get
            {
                if (httpPostCaller == null)
                {
                    httpPostCaller = new HttpPostCaller();
                }

                return httpPostCaller;
            }
        }
    }
}
